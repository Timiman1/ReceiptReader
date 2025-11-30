using Microsoft.AspNetCore.Mvc;
using ReceiptReader.Application.FileStorage;
using ReceiptReader.Application.Repositories;
using ReceiptReader.Application.Utility;
using ReceiptReader.Application.Validation;
using ReceiptReader.Application.Analyzers;
using ReceiptReader.Dtos;
using ReceiptReader.Mappers;
using ReceiptReader.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using ReceiptReader.Application.Exceptions;

namespace ReceiptReader.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly ILogger<FilesController> _logger;
        private readonly IFileStorage _fileStorage;
        private readonly IReceiptAnalyzer _receiptAnalyzer;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IFileValidator _fileValidator;
        private readonly IContentTypeResolver _contentTypeResolver;
        private readonly FileSettings _fileSettings;

        public FilesController(
            ILogger<FilesController> logger,
            IFileStorage fileStorage,
            IReceiptAnalyzer receiptAnalyzer,
            IReceiptRepository receiptRepository,
            IFileValidator fileValidator,
            IContentTypeResolver contentTypeResolver,
            IOptions<FileSettings> fileSettings)
        {
            _logger = logger;
            _fileStorage = fileStorage;
            _receiptAnalyzer = receiptAnalyzer;
            _receiptRepository = receiptRepository;
            _fileValidator = fileValidator;
            _contentTypeResolver = contentTypeResolver;
            _fileSettings = fileSettings.Value;
        }

        [HttpPost("upload")]
        [ProducesResponseType(typeof(ReceiptDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ReceiptDto>> UploadAsync(IFormFile file)
        {
            var maxFileSize = _fileSettings.MaxFileSizeInBytes;

            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            if (file.Length > maxFileSize)
            {
                return BadRequest($"File size exceeds the limit of {maxFileSize / 1024 / 1024} MB.");
            }
            if (!_fileValidator.HasAllowedExtension(file.FileName) ||
                !_fileValidator.IsAllowedMimeType(file.ContentType))
            {
                return BadRequest("Unsupported file format or content type.");
            }

            var fileId = Guid.NewGuid();
            using var stream = file.OpenReadStream();
            await _fileStorage.SaveAsync(
                stream,
                file.FileName,
                file.ContentType,
                fileId
            );

            using var ocrStream = await _fileStorage.OpenReadAsync(fileId);
            try
            {
                var analysisResult = await _receiptAnalyzer.AnalyzeAsync(ocrStream, fileId);
                return Ok(ReceiptMapper.ToDto(analysisResult.Receipt!));
            }
            catch (Exception ex)
            {
                await _receiptRepository.DeleteAsync(fileId);

                if (ex is ReceiptAnalyzerException)
                {
                    return BadRequest($"File content rejected: {ex.Message}");
                }

                _logger.LogError(ex, "An unexpected error occurred during receipt processing.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }

        [HttpGet("{fileId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(Guid fileId)
        {
            try
            {
                var extension = _fileStorage.TryGetExtension(fileId);

                if (extension == null) 
                {
                    return NotFound("File not found.");
                }

                var stream = await _fileStorage.OpenReadAsync(fileId);

                var contentType = _contentTypeResolver.GetContentType(extension)
                    ?? "application/octet-stream";

                return File(stream, contentType);
            }
            catch (FileNotFoundException)
            {
                return NotFound("File not found.");
            }
        }
    }
}
