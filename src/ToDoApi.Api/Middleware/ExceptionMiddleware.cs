using System.Net;
using System.Text.Json;
using ToDoApi.Domain;

namespace ToDoApi.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext); // Call next middleware in the pipeline if no exception occurs
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An unhandled exception has occurred: {Message}", ex.Message);

                // Handle the exception and create a custom response
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError; 

                // Create a standardized API response
                var apiResponse = new ApiResponse<string>(false, "An unexpected error occurred.", null);

                // if want to see orignal execption message
               //var apiResponse = new ApiResponse<string>(false, ex.Message, null);

                var jsonResponse = JsonSerializer.Serialize(apiResponse, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase // Match default JSON serialization
                });

                await httpContext.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
