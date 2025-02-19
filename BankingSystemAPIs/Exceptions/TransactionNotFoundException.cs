namespace BankingSystemAPIs.Exceptions
{
    // Custom exception for handling transaction not found scenarios
    public class TransactionNotFoundException : Exception
    {
        // Default constructor for the exception
        public TransactionNotFoundException()
        {
        }

        // Constructor that accepts a custom message to be passed to the base exception
        public TransactionNotFoundException(string message)
            : base(message) // Pass the custom message to the base exception class
        {
        }

        // Constructor that accepts both a custom message and an inner exception
        public TransactionNotFoundException(string message, Exception inner)
            : base(message, inner) // Pass both the message and inner exception to the base class
        {
        }
    }
}
