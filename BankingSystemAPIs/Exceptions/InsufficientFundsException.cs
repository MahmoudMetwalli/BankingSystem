namespace BankingSystemAPIs.Exceptions
{
    // Custom exception for handling insufficient funds scenario
    public class InsufficientFundsException : Exception
    {
        // Default constructor for the exception
        public InsufficientFundsException()
        {
        }

        // Constructor that accepts a custom message to be passed to the base exception
        public InsufficientFundsException(string message)
            : base(message) // Pass the custom message to the base Exception class
        {
        }

        // Constructor that accepts both a custom message and an inner exception
        public InsufficientFundsException(string message, Exception inner)
            : base(message, inner) // Pass both the message and inner exception to the base class
        {
        }
    }
}
