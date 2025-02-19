namespace BankingSystemAPIs.Exceptions
{
    // Custom exception for handling duplicated ID errors
    public class DuplicatedIdException : Exception
    {
        // Default constructor for the exception
        public DuplicatedIdException()
        {
        }

        // Constructor that accepts a custom message to be passed to the base exception
        public DuplicatedIdException(string message)
            : base(message) // Pass the custom message to the base Exception class
        {
        }

        // Constructor that accepts both a custom message and an inner exception
        public DuplicatedIdException(string message, Exception inner)
            : base(message, inner) // Pass both the message and inner exception to the base class
        {
        }
    }
}
