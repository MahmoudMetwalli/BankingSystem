using System.Net;
using System.Text.Json;
using BankingSystemAPIs.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BankingSystemAPIs.Middlewares
{
    // Middleware to handle global exceptions for the application
    public class GlobalExceptionHandler : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        // Constructor to inject a logger instance for logging exception details
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        // This method is called when the middleware is invoked
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // Generate a unique request ID for the incoming request to track it
            var requestId = Guid.NewGuid().ToString();
            context.Items["RequestId"] = requestId; // Store the request ID in the context for use downstream
            context.Response.Headers["X-Request-ID"] = requestId; // Include the request ID in the response headers for the client

            try
            {
                // Log the incoming request with its method and path
                _logger.LogInformation("Request {RequestId} Received: {Method} {Path}",
                                       requestId,
                                       context.Request.Method,
                                       context.Request.Path);

                // Proceed to the next middleware or handler
                await next(context);

                // Log the response status code after the request has been processed
                _logger.LogInformation("Response {RequestId} Sent: {StatusCode}",
                                       requestId,
                                       context.Response.StatusCode);
            }
            catch (AccountTypeException ex)
            {
                // Catch ClientNotFoundException and handle it with a custom error response
                await HandleExceptionAsync(context, HttpStatusCode.NotFound, ex.Message, ex, requestId);
            }
            catch (ClientNotFoundException ex)
            {
                // Catch ClientNotFoundException and handle it with a custom error response
                await HandleExceptionAsync(context, HttpStatusCode.NotFound, ex.Message, ex, requestId);
            }
            catch (DuplicatedIdException ex)
            {
                // Catch DuplicatedIdException and handle it with a custom error response
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message, ex, requestId);
            }
            catch (RateNotFoundException ex)
            {
                // Catch RateNotFoundException and handle it with a custom error response
                await HandleExceptionAsync(context, HttpStatusCode.NotFound, ex.Message, ex, requestId);
            }
            catch (NotPositiveException ex)
            {
                // Catch NotPositiveException and handle it with a custom error response
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message, ex, requestId);
            }
            catch (InsufficientFundsException ex)
            {
                // Catch InsufficientFundsException and handle it with a custom error response
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message, ex, requestId);
            }
            catch (AccountNotFoundException ex)
            {
                // Catch AccountNotFoundException and handle it with a custom error response
                await HandleExceptionAsync(context, HttpStatusCode.NotFound, ex.Message, ex, requestId);
            }
            catch (TransactionNotFoundException ex)
            {
                // Catch TransactionNotFoundException and handle it with a custom error response
                await HandleExceptionAsync(context, HttpStatusCode.NotFound, ex.Message, ex, requestId);
            }
            catch (AccountNumberException ex)
            {
                // Catch AccountNumberException and handle it with a custom error response
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message, ex, requestId);
            }
            catch (AsynchronizationException ex)
            {
                // Catch AsynchronizationException and handle it with a custom error response
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message, ex, requestId);
            }
            catch (DbUpdateException ex)
            {
                // Catch DbUpdateException (DB errors) and handle it with a custom error response
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, "Invalid Data", ex, requestId);
            }
            catch (Exception ex)
            {
                // Catch any general exception and handle it as a server error
                await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, "Server Error, Please Try Again Later", ex, requestId);
            }
        }

        // Helper method to handle exceptions and generate a structured error response
        private async Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message, Exception exception, string requestId)
        {
            // Log the error details with the request ID and the exception message
            _logger.LogError(exception, "Error in Request {RequestId}: {Message}", requestId, message);

            // Set the response content type and status code
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            // Write the error details in a JSON format
            await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorDetails()
            {
                Code = context.Response.StatusCode.ToString(), // Set the error code
                Message = message, // Set the error message
                RequestId = requestId // Include the request ID for reference
            }));
        }
    }

    // ErrorDetails class to represent the structure of the error response
    public class ErrorDetails
    {
        public string Code { get; set; } = null!; // Error code (status code as string)
        public string Message { get; set; } = null!; // Error message
        public string RequestId { get; set; } = null!; // The unique request ID to trace the error
    }
}
