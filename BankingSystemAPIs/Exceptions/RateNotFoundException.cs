namespace BankingSystemAPIs.Exceptions
{
    // Custom exception for handling the scenario where a rate is not found
    public class RateNotFoundException : Exception
    {
        // Default constructor for the exception
        public RateNotFoundException()
        {
        }

        // Constructor that accepts a custom message to be passed to the base exception
        public RateNotFoundException(string message)
            : base(message) // Pass the custom message to the base exception class
        {
        }

        // Constructor that accepts both a custom message and an inner exception
        public RateNotFoundException(string message, Exception inner)
            : base(message, inner) // Pass both the message and inner exception to the base class
        {
        }
    }
}
