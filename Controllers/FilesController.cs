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

            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(ReceiptMapper.ToDto(result.Receipt!));
        }

        [HttpGet("{fileId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAsync(Guid fileId)
        {
            var result = await _fileRetrievalService.GetFileAsync(fileId);

            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }

            return File(result.FileStream!, result.ContentType!);
        }
    }
}
