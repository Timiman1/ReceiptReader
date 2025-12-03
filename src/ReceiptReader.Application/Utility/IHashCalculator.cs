namespace ReceiptReader.Application.Utility
{
    /// <summary>
    /// Represents the service that calculates file hashes for caching needs.
    /// </summary>
    public interface IHashCalculator
    {
        // Method summary is skipped, as the interface summary defines the contract.
        string CalculateHash(MemoryStream ms);
    }
}
