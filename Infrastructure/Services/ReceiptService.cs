using ReceiptReader.Application.Analyzers;
using ReceiptReader.Application.Exceptions;
using ReceiptReader.Application.FileStorage;
using ReceiptReader.Application.Repositories;
using ReceiptReader.Application.Utility;
using ReceiptReader.Application.Validation;
using ReceiptReader.Infrastructure.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ReceiptReader.Application.Services
{
    /// <summary>
    /// Implementation of receipt processing service.
    /// Handles file validation, storage, and analysis workflow.
    /// </summary>
    public class ReceiptService : IReceiptService
    {
        private readonly ILogger<ReceiptService> _logger;
        private readonly IFileStorage _fileStorage;
        private readonly IReceiptAnalyzer _receiptAnalyzer;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IFileValidator _fileValidator;
        private readonly FileSettings _fileSettings;

        public ReceiptService(
            ILogger<ReceiptService> logger,
            IFileStorage fileStorage,
            IReceiptAnalyzer receiptAnalyzer,
            IReceiptRepository receiptRepository,
            IFileValidator fileValidator,
            IOptions<FileSettings> fileSettings)
        {
            _logger = logger;
            _fileStorage = fileStorage;
            _receiptAnalyzer = receiptAnalyzer;
            _receiptRepository = receiptRepository;
            _fileValidator = fileValidator;
            _fileSettings = fileSettings.Value;
        }

        public async Task<ReceiptServiceResult> ProcessReceiptAsync(
            Stream fileStream, 
            string fileName, 
            string contentType, 
            long fileLength)
        {
            // Validate file size
            var maxFileSize = _fileSettings.MaxFileSizeInBytes;
            if (fileLength > maxFileSize)
            {
                return new ReceiptServiceResult
                {
                    IsSuccess = false,
                    ErrorMessage = $"File size exceeds the limit of {maxFileSize / 1024 / 1024} MB."
                };
            }

            // Validate file type
            if (!_fileValidator.HasAllowedExtension(fileName) ||
                !_fileValidator.IsAllowedMimeType(contentType))
            {
                return new ReceiptServiceResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Unsupported file format or content type."
                };
            }

            // Save file
            var fileId = Guid.NewGuid();
            await _fileStorage.SaveAsync(fileStream, fileName, contentType, fileId);

            // Analyze receipt
            try
            {
                using var ocrStream = await _fileStorage.OpenReadAsync(fileId);
                var analysisResult = await _receiptAnalyzer.AnalyzeAsync(ocrStream, fileId);
                
                return new ReceiptServiceResult
                {
                    IsSuccess = true,
                    Receipt = analysisResult.Receipt
                };
            }
            catch (Exception ex)
            {
                // Clean up file on failure
                await _receiptRepository.DeleteAsync(fileId);

                if (ex is ReceiptAnalyzerException)
                {
                    return new ReceiptServiceResult
                    {
                        IsSuccess = false,
                        ErrorMessage = $"File content rejected: {ex.Message}"
                    };
                }

                _logger.LogError(ex, "An unexpected error occurred during receipt processing.");
                return new ReceiptServiceResult
                {
                    IsSuccess = false,
                    ErrorMessage = "An unexpected error occurred."
                };
            }
        }
    }
}
