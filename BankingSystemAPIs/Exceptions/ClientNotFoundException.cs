namespace BankingSystemAPIs.Exceptions
{
    // Custom exception for handling client not found errors
    public class ClientNotFoundException : Exception
    {
        // Default constructor for the exception
        public ClientNotFoundException()
        {
        }

        // Constructor that accepts a custom message to be passed to the base exception
        public ClientNotFoundException(string message)
            : base(message) // Pass the custom message to the base Exception class
        {
        }

        // Constructor that accepts both a custom message and an inner exception
        public ClientNotFoundException(string message, Exception inner)
            : base(message, inner) // Pass both the message and inner exception to the base class
        {
        }
    }
}
