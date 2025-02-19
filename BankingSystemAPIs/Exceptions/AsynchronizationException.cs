using Microsoft.EntityFrameworkCore;

namespace BankingSystemAPIs.Exceptions
{
    // Custom exception that inherits from DbUpdateConcurrencyException to handle asynchronous synchronization issues
    public class AsynchronizationException : DbUpdateConcurrencyException
    {
        // Default constructor for the exception
        public AsynchronizationException()
        {
        }

        // Constructor that accepts a custom message to be passed to the base exception
        public AsynchronizationException(string message)
            : base(message) // Pass the custom message to the base DbUpdateConcurrencyException class
        {
        }

        // Constructor that accepts both a custom message and an inner exception
        public AsynchronizationException(string message, Exception inner)
            : base(message, inner) // Pass both the message and inner exception to the base class
        {
        }
    }
}
