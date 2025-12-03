namespace ReceiptReader.Domain.Shared
{
    public enum FailureType
    {
        None,
        ValidationError, // Used for invalid user input/data
        SystemError      // Used for unexpected infrastructure problems
    }
}
