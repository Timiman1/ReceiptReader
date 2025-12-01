using ReceiptReader.Application.Analyzers;
using ReceiptReader.Application.Exceptions;
using ReceiptReader.Application.FileStorage;
using ReceiptReader.Application.Repositories;
using ReceiptReader.Application.Services;
using ReceiptReader.Application.Utility;
using ReceiptReader.Application.Validation;
using ReceiptReader.Infrastructure.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ReceiptReader.Infrastructure.Services
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
                return new ReceiptServiceFailure(
                    $"File size exceeds the limit of {maxFileSize / 1024 / 1024} MB.");
            }

            // Validate file type
            if (!_fileValidator.HasAllowedExtension(fileName) ||
                !_fileValidator.IsAllowedMimeType(contentType))
            {
                return new ReceiptServiceFailure(
                    "Unsupported file format or content type.");
            }

            // Save file
            var fileId = Guid.NewGuid();
            await _fileStorage.SaveAsync(fileStream, fileName, contentType, fileId);

            // Analyze receipt
            try
            {
                using var ocrStream = await _fileStorage.OpenReadAsync(fileId);
                var analysisResult = await _receiptAnalyzer.AnalyzeAsync(ocrStream, fileId);
                
                if (analysisResult == null)
                {
                    _logger.LogError("Analyzer returned null result for fileId {FileId}", fileId);
                    return new ReceiptServiceFailure(
                        "Analysis failed - analyzer returned no result.");
                }

                if (analysisResult.Receipt == null)
                {
                    _logger.LogError("Analysis completed but no receipt was returned for fileId {FileId}", fileId);
                    return new ReceiptServiceFailure(
                        "Analysis completed but no receipt data was extracted.");
                }
                
                return new ReceiptServiceSuccess(analysisResult.Receipt);
            }
            catch (Exception ex)
            {
                // Attempt to clean up any database records that may have been created
                // Note: File remains in storage intentionally for debugging/reprocessing
                // Note: This is a safe operation - DeleteAsync does nothing if receipt doesn't exist
                try
                {
                    await _receiptRepository.DeleteAsync(fileId);
                }
                catch (Exception cleanupEx)
                {
                    _logger.LogError(cleanupEx, "Failed to clean up receipt record for fileId {FileId} after analysis failure.", fileId);
                    // Continue with original error handling - cleanup failure is secondary
                }

                if (ex is ReceiptAnalyzerException)
                {
                    return new ReceiptServiceFailure(
                        $"File content rejected: {ex.Message}");
                }

                _logger.LogError(ex, "An unexpected error occurred during receipt processing.");
                return new ReceiptServiceFailure(
                    "An unexpected error occurred.");
            }
        }
    }
}
