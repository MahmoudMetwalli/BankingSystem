using System.Reflection;
using BankingSystemAPIs.Data;
using BankingSystemAPIs.Entities.User;
using BankingSystemAPIs.Middlewares;
using BankingSystemAPIs.Repository;
using BankingSystemAPIs.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLogging(config =>
{
    // Configure logging to output to both the console and debug
    config.AddConsole();
    config.AddDebug();
});

// Add custom global exception handler middleware to the DI container
builder.Services.AddTransient<GlobalExceptionHandler>();

// Add controllers to handle API requests
builder.Services.AddControllers();

// Add OpenAPI/Swagger documentation generation to support interactive API documentation
builder.Services.AddEndpointsApiExplorer();

// Setup OpenAPI (Swagger) details
var info = new OpenApiInfo()
{
    Title = "Banking System API Documentation", // API title
    Version = "v1", // API version
    Description = "The Banking System API provides a comprehensive set of endpoints for managing banking-related operations. It is designed to support various banking activities such as account management, transaction processing, and exchange rate management. This API is built with scalability and security in mind, ensuring seamless integration with client applications.\r\n\r\nKey Features:\r\nAccount Management: Create, retrieve, update, and delete accounts. Perform operations such as deposits, withdrawals, and transfers.\r\nTransaction Processing: Retrieve transaction histories, manage transaction records, and delete transactions securely.\r\nExchange Rates: Manage exchange rates for supported currencies, including retrieval, creation, updates, and deletions.\r\nLogging: Detailed request and response logging to facilitate debugging and auditing.\r\nRESTful Design: Follows RESTful principles with predictable endpoints, standard HTTP methods, and meaningful status codes.\r\nSwagger Documentation: Integrated Swagger/OpenAPI support for testing and visualizing API capabilities.",
    Contact = new OpenApiContact()
    {
        Name = "Mahmoud Metwalli", // Contact person for the API
        Email = "mammkbih@gmail.com",
    }
};

// Add Swagger generation services to the DI container
builder.Services.AddSwaggerGen(c =>
{
    // Set Swagger documentation details
    c.SwaggerDoc("v1", info);

    // Set the comments path for the Swagger JSON and UI (XML comments for API documentation)
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Configure the DbContext for the application to use PostgreSQL
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresqlString"));
});
builder.Services.AddScoped<ClientRepository>();
builder.Services.AddScoped<RateRepository>();
builder.Services.AddScoped<TransactionRepository>();
builder.Services.AddScoped<AccountTransactionRepository>();
builder.Services.AddScoped<AccountRepository>();

// Register application services
builder.Services.AddScoped<AccountTransactionService>();
builder.Services.AddScoped<ClientService>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<TransactionService>();
builder.Services.AddScoped<RateService>();

// Build the application
var app = builder.Build();

// Use the global exception handler middleware for the app
app.UseMiddleware<GlobalExceptionHandler>();

// Configure the HTTP request pipeline (Swagger UI setup for development environment)
if (app.Environment.IsDevelopment())
{
    // Setup Swagger UI for development
    app.UseSwagger(u =>
    {
        u.RouteTemplate = "swagger/{documentName}/swagger.json";
    });

    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = "swagger"; // Set the route prefix for Swagger UI
        c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "Banking System API Documentation");
    });
}

// Enable HTTPS redirection for secure communication
app.UseHttpsRedirection();

// Enable authorization middleware
app.UseAuthorization();

// Map API controllers to routes
app.MapControllers();

// Run the application
app.Run();
