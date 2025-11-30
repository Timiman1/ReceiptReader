using ReceiptReader.Application.FileStorage;
using ReceiptReader.Application.Validation;
using System.IO;

namespace ReceiptReader.Infrastructure.FileStorage
{
    public class LocalFileStorage : IFileStorage
    {
        private readonly string _root;

        public LocalFileStorage(
            IWebHostEnvironment env,
            IFileValidator fileValidator)
        {
            _root = Path.Combine(env.WebRootPath, "uploads");

            if (Directory.Exists(_root) == false)
            {
                Directory.CreateDirectory(_root);
            }
        }

        public async Task SaveAsync(
            Stream stream,
            string fileName,
            string contentType,
            Guid fileId)
        {
            var ext = Path.GetExtension(fileName);
            var storedName = fileId + ext;
            var path = Path.Combine(_root, storedName);

            using var fileStream = new FileStream(
                path,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None
            );

            await stream.CopyToAsync(fileStream);
        }

        public Task<Stream> OpenReadAsync(Guid fileId)
        {
            var filePath = Directory.GetFiles(_root)
                .FirstOrDefault(f => 
                    Path.GetFileNameWithoutExtension(f)
                        .Equals(fileId.ToString(), StringComparison.OrdinalIgnoreCase));

            if (filePath == null)
            {
                throw new FileNotFoundException("File not found", fileId.ToString());
            }

            Stream stream = new FileStream(
                filePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read
            );

            return Task.FromResult(stream);
        }

        public string? TryGetExtension(Guid fileId)
        {
            var matchingFilePath = Directory.GetFiles(_root)
                .FirstOrDefault(f =>
                    Path.GetFileNameWithoutExtension(f)
                        .Equals(fileId.ToString(), StringComparison.OrdinalIgnoreCase));

            if (matchingFilePath == null) return null;

            return Path.GetExtension(matchingFilePath);
        }
    }
}
