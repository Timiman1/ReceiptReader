using Microsoft.Extensions.Options;
using ReceiptReader.Application.Exceptions;
using ReceiptReader.Application.RawTextExtractors;
using ReceiptReader.Infrastructure.Configurations;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Tesseract;

namespace ReceiptReader.Infrastructure.RawTextExtractors
{
    public class TesseractRawTextExtractor : ILowCostRawTextExtractor
    {
        private readonly ILogger<TesseractRawTextExtractor> _logger;
        private readonly TesseractOptions _options;

        public TesseractRawTextExtractor(
            ILogger<TesseractRawTextExtractor> logger,
            IOptions<TesseractOptions> options)
        {
            _logger = logger;
            _options = options.Value;
        }

        public async Task<string> GetRawTextAsync(MemoryStream ms)
        {
            var tessdataPath = Path.Combine(AppContext.BaseDirectory, "Infrastructure", "tessdata");

            try
            {
                using (var imageSharpImage = Image.Load<Rgba32>(ms))
                {
                    // Grayscale reduces complexity and helps with binarization.
                    imageSharpImage.Mutate(x => x.Grayscale());

                    // Binarization creates a purely white/black image. Optimal for OCR.
                    imageSharpImage.Mutate(x => x.BinaryThreshold(0.5f));

                    using (var tempMs = new MemoryStream())
                    {
                        // TIFF/PNG is the optimal format for Tesseract.
                        await imageSharpImage.SaveAsTiffAsync(tempMs); 

                        tempMs.Position = 0;

                        using (var engine = new TesseractEngine(tessdataPath, _options.Language, EngineMode.Default))
                        using (var img = Pix.LoadFromMemory(tempMs.ToArray()))

                        // SparseText: Used for unstructured receipts where text is diffuse.
                        using (var page = engine.Process(img, PageSegMode.SparseText)) 
                        {
                            return page.GetText();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Tesseract (Low-cost OCR) failed during raw text extraction or image preprocessing.");

                throw new RawTextExtractionException("Low-cost raw text extraction (Tesseract) failed due to an underlying OCR or file processing error.", ex);
            }
        }
    }
}
