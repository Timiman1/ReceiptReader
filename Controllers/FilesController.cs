using Microsoft.AspNetCore.Mvc;
using ReceiptReader.Application.FileStorage;
using ReceiptReader.Application.Utility;
using ReceiptReader.Dtos;
using ReceiptReader.Application.Services;
using ReceiptReader.Application.Commands;
using ReceiptReader.Domain.Shared;

namespace ReceiptReader.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly ILogger<FilesController> _logger;
        private readonly IFileStorage _fileStorage;
        private readonly IContentTypeResolver _contentTypeResolver;
        private readonly IReceiptProcessingService _receiptProcessingService;

        public FilesController(
            ILogger<FilesController> logger,
            IFileStorage fileStorage,
            IContentTypeResolver contentTypeResolver,
            IReceiptProcessingService receiptProcessingService)
        {
            _logger = logger;
            _fileStorage = fileStorage;
            _contentTypeResolver = contentTypeResolver;
            _receiptProcessingService = receiptProcessingService;
        }

        [HttpPost("upload")]
        [ProducesResponseType(typeof(ReceiptDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ReceiptDto>> UploadAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var stream = file.OpenReadStream();
            var result = await _receiptProcessingService.ProcessReceiptAsync(
                new ProcessReceiptCommand(stream, file.FileName, file.ContentType, file.Length)
            );

            if (result.IsSuccess)
            {
                return result.Value;
            }
            else
            {
                var error = result.FailureType switch
                {
                    FailureType.ValidationError => BadRequest(result.ErrorMessage),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
                };

                return error;
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
