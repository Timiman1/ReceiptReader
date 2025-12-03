namespace ReceiptReader.Application.Commands
{
    public record ProcessReceiptCommand(
        Stream FileStream,
        string FileName,
        string ContentType,
        long FileLength);
}
