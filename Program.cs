// Importing necessary namespaces for Entity Framework Core and our Todo models
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

// Creating a web application builder with command-line arguments
var builder = WebApplication.CreateBuilder(args);

// Adding API explorer services for documentation endpoints
builder.Services.AddEndpointsApiExplorer();

// Adding Swagger generation services for API documentation
builder.Services.AddSwaggerGen();

// Adding Entity Framework DbContext to the service container with SQL Server configuration
builder.Services.AddDbContext<TodoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TodoDb")));

// CORS Configuration section
builder.Services.AddCors(options =>
{
    // Adding a CORS policy named "AllowFrontend"
    options.AddPolicy("AllowFrontend",
        policy => policy
            // Specifying which origins are allowed to access the API
            .WithOrigins(
                "http://localhost:5173", // Vite development server
                "https://localhost:5173"  // Secure Vite development server
            )
            // Allowing any HTTP headers in requests
            .AllowAnyHeader()
            // Allowing any HTTP methods (GET, POST, PUT, DELETE, etc.)
            .AllowAnyMethod()
            // Allowing credentials (cookies, authorization headers) to be sent
            .AllowCredentials());
});

// Building the web application
var app = builder.Build();

// Checking if the application is running in Development environment
if (app.Environment.IsDevelopment())
{
    // Enabling Swagger middleware for API documentation
    app.UseSwagger();
    // Enabling Swagger UI middleware for interactive API documentation
    app.UseSwaggerUI();
}

// Enabling HTTPS redirection middleware
app.UseHttpsRedirection();

// Enabling CORS middleware with the "AllowFrontend" policy
app.UseCors("AllowFrontend");

// Enabling authorization middleware
app.UseAuthorization();

// Mapping controller routes
app.MapControllers();

// Running the web application
app.Run();