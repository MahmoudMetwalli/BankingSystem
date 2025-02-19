namespace BankingSystemAPIs.Exceptions
{
    // Custom exception to handle errors related to account numbers
    public class AccountNumberException : Exception
    {
        // Default constructor for the exception
        public AccountNumberException()
        {
        }

        // Constructor that accepts a custom message to be passed to the base exception
        public AccountNumberException(string message)
            : base(message) // Pass the custom message to the base Exception class
        {
        }

        // Constructor that accepts both a custom message and an inner exception
        public AccountNumberException(string message, Exception inner)
            : base(message, inner) // Pass both the message and inner exception to the base class
        {
        }
    }
}
