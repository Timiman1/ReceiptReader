namespace ReceiptReader.Application.Utility
{
    public class HashCalculator : IHashCalculator
    {
        public string CalculateHash(MemoryStream ms)
        {
            ms.Position = 0;

            using var sha256 = System.Security.Cryptography.SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(ms);

            return Convert.ToHexStringLower(hashBytes);
        }
    }
}
