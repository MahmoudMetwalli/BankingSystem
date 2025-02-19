namespace BankingSystemAPIs.Exceptions
{
    // Custom exception to handle errors when an account is not found
    public class AccountNotFoundException : Exception
    {
        // Default constructor for the exception
        public AccountNotFoundException()
        {
        }

        // Constructor that accepts a custom message to be passed to the base exception
        public AccountNotFoundException(string message)
            : base(message) // Pass the custom message to the base Exception class
        {
        }

        // Constructor that accepts both a custom message and an inner exception
        public AccountNotFoundException(string message, Exception inner)
            : base(message, inner) // Pass both the message and inner exception to the base class
        {
        }
    }
}
