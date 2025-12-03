using ReceiptReader.Application.Analyzers;
using ReceiptReader.Application.Exceptions;
using ReceiptReader.Application.RawTextExtractors;
using ReceiptReader.Application.ReceiptDataExtractors;
using ReceiptReader.Application.Repositories;
using ReceiptReader.Application.Utility;
using ReceiptReader.Domain.Entities;

namespace ReceiptReader.Infrastructure.Analyzers
{
    public class DefaultReceiptAnalyzer : IReceiptAnalyzer
    {
        private readonly ILogger<DefaultReceiptAnalyzer> _logger;
        private readonly IAnalysisLogRepository _analysisLogRepository;
        private readonly IReceiptRepository _receiptRepository;
        private readonly ILowCostRawTextExtractor _lowCostOcrFilter;
        private readonly IHighCostRawTextExtractor _highCostOcrFilter;
        private readonly IReceiptDataExtractor _receiptDataExtractor;
        private readonly IHashCalculator _hashCalculator;
        private readonly IReceiptInfoMapper _receiptInfoMapper;
        private readonly IReceiptKeywordClassifier _receiptKeywordClassifier;

        public DefaultReceiptAnalyzer(
            ILogger<DefaultReceiptAnalyzer> logger,
            IAnalysisLogRepository analysisLogRepository,
            IReceiptRepository receiptRepository,
            ILowCostRawTextExtractor lowCostOcrFilter,
            IHighCostRawTextExtractor highCostOcrFilter,
            IReceiptDataExtractor receiptDataExtractor,
            IHashCalculator hashCalulator,
            IReceiptInfoMapper receiptInfoMapper,
            IReceiptKeywordClassifier receiptKeywordClassifier)
        {
            _logger = logger;
            _analysisLogRepository = analysisLogRepository;
            _receiptRepository = receiptRepository;
            _lowCostOcrFilter = lowCostOcrFilter;
            _highCostOcrFilter = highCostOcrFilter;
            _receiptDataExtractor = receiptDataExtractor;
            _hashCalculator = hashCalulator;
            _receiptInfoMapper = receiptInfoMapper;
            _receiptKeywordClassifier = receiptKeywordClassifier;
        }

        public async Task<AnalysisResult> AnalyzeAsync(Stream stream, Guid fileId)
        {
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            ms.Position = 0;

            string fileHash = _hashCalculator.CalculateHash(ms);

            var hashLogEntry = await _analysisLogRepository.GetLogEntryByHashAsync(fileHash);

            if (!string.IsNullOrEmpty(hashLogEntry?.FailureReason))
            {
                throw new ReceiptAnalyzerException(hashLogEntry.FailureReason);
            }

            if (hashLogEntry?.Status == AnalysisStatus.Completed)
            {
                if (hashLogEntry.ReceiptInfo != null)
                {
                    return new AnalysisResult { Receipt = hashLogEntry.ReceiptInfo, IsCached = true };
                }
                else
                {
                    _logger.LogError(
                        $"Application Error: Found completed AnalysisLog (FileHash: {fileHash}) with Status='Completed' but its navigation property ReceiptInfo was null. The cached analysis result is unusable.",
                        hashLogEntry.FileHash);
                }
            }

            ms.Position = 0;
            string lowCostRawOcrText = await _lowCostOcrFilter.GetRawTextAsync(ms);
            
            if (!_receiptKeywordClassifier.IsReceiptLikely(lowCostRawOcrText))
            {
                var reason = "Input document failed the low cost content filter check. Expected key financial/date keywords were not detected.";
                await LogFailureAndThrowAsync(fileHash, AnalysisStatus.FilteredOut, reason);
            }

            ms.Position = 0;
            string highCostRawOcrText = await _highCostOcrFilter.GetRawTextAsync(ms);

            if (!_receiptKeywordClassifier.IsReceiptLikely(highCostRawOcrText))
            {
                var reason = "Input document failed the high cost content filter check. Expected key financial/date keywords were not detected.";
                await LogFailureAndThrowAsync(fileHash, AnalysisStatus.FilteredOut, reason);
            }

            try
            {
                var extractionResult = await _receiptDataExtractor.ExtractDataAsync(highCostRawOcrText);
                var receipt = _receiptInfoMapper.MapToDomainModel(extractionResult, fileId);
                await _receiptRepository.AddAsync(receipt);

                var completedLogEntry = new AnalysisLog
                {
                    FileHash = fileHash,
                    Status = AnalysisStatus.Completed,
                    AnalysisDate = DateTime.UtcNow,
                    ReceiptInfoId = receipt.FileId
                };

                await _analysisLogRepository.AddLogAsync(completedLogEntry);

                return new AnalysisResult { Receipt = receipt, IsCached = false };
            }
            catch (ReceiptDataExtractionException ex)
            {
                var reason = ex.Message;
                await LogFailureAndThrowAsync(fileHash, AnalysisStatus.DataExtractionFailed, reason);

                throw;
            }
            catch (Exception ex)
            {
                // Catch all unexpected errors (DB issues, mapping errors, etc.)
                var reason = $"An unexpected critical error occurred during analysis: {ex.Message}";

                await LogFailureAndThrowAsync(fileHash, AnalysisStatus.CriticalFailure, reason);

                throw;
            }
        }

        private async Task LogFailureAndThrowAsync(
            string fileHash,
            AnalysisStatus status,
            string reason)
        {
            _logger.LogError(
                $"Analysis failed (FileHash: {fileHash}, Status: {status}). Reason: {reason}",
                fileHash, status, reason);

            var logEntry = new AnalysisLog
            {
                FileHash = fileHash,
                Status = status,
                FailureReason = reason,
                AnalysisDate = DateTime.UtcNow
            };
            await _analysisLogRepository.AddLogAsync(logEntry);

            throw new ReceiptAnalyzerException(reason);
        }
    }
}
