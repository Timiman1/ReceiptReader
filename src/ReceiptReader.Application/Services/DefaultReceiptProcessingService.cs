using Microsoft.Extensions.Options;
using ReceiptReader.Application.Analyzers;
using ReceiptReader.Application.Commands;
using ReceiptReader.Application.Exceptions;
using ReceiptReader.Application.FileStorage;
using ReceiptReader.Application.Repositories;
using ReceiptReader.Application.Utility;
using ReceiptReader.Application.Validation;
using ReceiptReader.Domain.Shared;
using ReceiptReader.Application.Dtos;
using ReceiptReader.Application.Configurations;
using ReceiptReader.Application.Mappers;

namespace ReceiptReader.Application.Services
{
    public class DefaultReceiptProcessingService : IReceiptProcessingService
    {
        private readonly ILogger<DefaultReceiptProcessingService> _logger;
        private readonly IFileStorage _fileStorage;
        private readonly IReceiptAnalyzer _receiptAnalyzer;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IFileValidator _fileValidator;
        private readonly FileSettings _fileSettings;

        public DefaultReceiptProcessingService(
            ILogger<DefaultReceiptProcessingService> logger, 
            IFileStorage fileStorage,
            IReceiptAnalyzer receiptAnalyzer, 
            IReceiptRepository receiptRepository,
            IFileValidator fileValidator, IContentTypeResolver contentTypeResolver,
            IOptions<FileSettings> fileSettings)
        {
            _logger = logger;
            _fileStorage = fileStorage;
            _receiptAnalyzer = receiptAnalyzer;
            _receiptRepository = receiptRepository;
            _fileValidator = fileValidator;
            _fileSettings = fileSettings.Value;
        }

        public async Task<ResultT<ReceiptDto>> ProcessReceiptAsync(ProcessReceiptCommand command)
        {
            var maxFileSize = _fileSettings.MaxFileSizeInBytes;
            if (command.FileLength > maxFileSize)
            {
                return ResultT<ReceiptDto>.Fail(
                    $"File size exceeds the limit of {maxFileSize / 1024 / 1024} MB.", 
                    FailureType.ValidationError);
            }
            if (!_fileValidator.HasAllowedExtension(command.FileName) ||
                !_fileValidator.IsAllowedMimeType(command.ContentType))
            {
                return ResultT<ReceiptDto>.Fail(
                    "Unsupported file format or content type.",
                    FailureType.ValidationError);
            }

            var fileId = Guid.NewGuid();

            try
            {
                await _fileStorage.SaveAsync(
                    command.FileStream,
                    command.FileName,
                    command.ContentType,
                    fileId);

                using var ocrStream = await _fileStorage.OpenReadAsync(fileId);
                var analysisResult = await _receiptAnalyzer.AnalyzeAsync(ocrStream, fileId);

                return ResultT<ReceiptDto>.Success(ReceiptMapper.ToDto(analysisResult.Receipt!));
            }
            catch (ReceiptAnalyzerException ex)
            {
                await _receiptRepository.DeleteAsync(fileId);
                return ResultT<ReceiptDto>.Fail(
                    $"File content rejected: {ex.Message}",
                    FailureType.SystemError);
            }
            catch (Exception ex)
            {
                await _receiptRepository.DeleteAsync(fileId);
                _logger.LogError(ex, "Unexpected error during processing of {FileId}", fileId);
                return ResultT<ReceiptDto>.Fail(
                    "An unexpected error occurred.",
                    FailureType.SystemError);
            }
        }
    }
}
