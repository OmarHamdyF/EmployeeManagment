using EmployeeManagement.Application.Common.Exceptions;
using System.Net;
using System.Text.Json;

namespace EmployeeManagment.API.Common
{
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception has occurred.");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var statusCode = HttpStatusCode.InternalServerError;
            var message = "An internal server error occurred.";
            var errors = new Dictionary<string, string[]>();

            switch (exception)
            {
                case ValidationException validationException:
                    statusCode = HttpStatusCode.BadRequest;
                    message = "Validation Error";

                    foreach (var errorEntry in validationException.Errors) // Renamed 'error' to 'errorEntry' for clarity
                    {
                        if (!errors.ContainsKey(errorEntry.Key))
                        {
                            errors[errorEntry.Key] = errorEntry.Value; 
                        }
                        else
                        {
                            // If the key exists, concatenate the existing array with the new array
                            errors[errorEntry.Key] = errors[errorEntry.Key].Concat(errorEntry.Value).ToArray(); // FIX: Use Concat
                        }
                    }
                    break;
                case NotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    message = notFoundException.Message;
                    break;
                case ArgumentException argEx:
                    statusCode = HttpStatusCode.BadRequest;
                    message = argEx.Message;
                    break;
                default:
                    // Log the full exception details for debugging
                    break;
            }

            context.Response.StatusCode = (int)statusCode;
            var response = new
            {
                StatusCode = (int)statusCode,
                Message = message,
                Errors = errors.Any() ? errors : null // Only include errors if there are validation errors
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }

}
