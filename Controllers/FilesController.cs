using Microsoft.AspNetCore.Mvc;
using ReceiptReader.Application.Services;
using ReceiptReader.Dtos;
using ReceiptReader.Mappers;

namespace ReceiptReader.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IReceiptService _receiptService;
        private readonly IFileRetrievalService _fileRetrievalService;

        public FilesController(
            IReceiptService receiptService,
            IFileRetrievalService fileRetrievalService)
        {
            _receiptService = receiptService;
            _fileRetrievalService = fileRetrievalService;
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

            using var stream = file.OpenReadStream();
            var result = await _receiptService.ProcessReceiptAsync(
                stream, 
                file.FileName, 
                file.ContentType, 
                file.Length);

            return result switch
            {
                ReceiptServiceSuccess success => Ok(ReceiptMapper.ToDto(success.Receipt)),
                ReceiptServiceFailure failure => BadRequest(failure.ErrorMessage),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "Unknown error occurred")
            };
        }

        [HttpGet("{fileId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(Guid fileId)
        {
            var result = await _fileRetrievalService.GetFileAsync(fileId);

            return result switch
            {
                FileRetrievalSuccess success => File(success.FileStream, success.ContentType),
                FileRetrievalFailure failure => NotFound(failure.ErrorMessage),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "Unknown error occurred")
            };
        }
    }
}
