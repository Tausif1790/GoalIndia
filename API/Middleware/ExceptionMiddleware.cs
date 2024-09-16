using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware;

//This middleware is designed to handle exceptions globally in your API. It intercepts any exceptions
// that occur during the execution of an HTTP request and ensures that a proper error response is sent
// back to the client. The middleware can be particularly useful for maintaining consistent error handling
// across the application and for providing detailed error information in development environments
// while keeping the response generic in production.
public class ExceptionMiddleware
{
    private readonly IHostEnvironment _env;       // Helps to determine if we're in Development or Production
    private readonly RequestDelegate _next;       // Represents the next middleware in the pipeline

    // Constructor takes IHostEnvironment to check the environment and RequestDelegate to pass the request to the next middleware
    public ExceptionMiddleware(IHostEnvironment env, RequestDelegate next)
    {
        _env = env;       // Stores the environment (development, production, etc.)
        _next = next;     // Stores the next middleware delegate
    }

    // Method to invoke the middleware in the pipeline
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);  // Passes the request to the next middleware in the pipeline
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, _env);      // If an exception occurs, handle it by calling the HandleExceptionAsync method
        }
    }

    // Handles the exception and sends a custom error response to the client
    private static Task HandleExceptionAsync(HttpContext context, Exception ex, IHostEnvironment env)
    {
        context.Response.ContentType = "application/json";  // Sets the response content type to JSON
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;  // Sets the HTTP status code to 500 (Internal Server Error)

        // Create an ApiErrorResponse based on whether we're in development or production
        var response = env.IsDevelopment()
            ? new ApiErrorResponse(context.Response.StatusCode, ex.Message, ex.StackTrace)                  // If in development, include detailed error message and stack trace
            : new ApiErrorResponse(context.Response.StatusCode, ex.Message, "Internal server error");       // If in production, hide the details and show a generic message


        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };      // Options for the JSON serializer to use camelCase for property names

        var json = JsonSerializer.Serialize(response, options);             // Serialize the ApiErrorResponse object to JSON format

        return context.Response.WriteAsync(json);                           // Write the JSON response back to the client
    }
}