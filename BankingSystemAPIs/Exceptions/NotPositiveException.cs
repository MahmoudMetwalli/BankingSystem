namespace BankingSystemAPIs.Exceptions
{
    // Custom exception for handling the scenario where a value is not positive
    public class NotPositiveException : ArgumentException
    {
        // Default constructor for the exception
        public NotPositiveException()
        {
        }

        // Constructor that accepts a custom message to be passed to the base exception
        public NotPositiveException(string message)
            : base(message) // Pass the custom message to the base ArgumentException class
        {
        }

        // Constructor that accepts both a custom message and an inner exception
        public NotPositiveException(string message, Exception inner)
            : base(message, inner) // Pass both the message and inner exception to the base class
        {
        }
    }
}
