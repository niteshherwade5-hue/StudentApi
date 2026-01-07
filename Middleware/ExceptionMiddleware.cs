using System.Net;
using System.Text.Json;
using WebApplication1.Exceptions;

namespace WebApplication1.Middleware
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

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");

                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            var message = "An unexpected error occurred";

            if (exception is NotFoundException)
            {
                statusCode = HttpStatusCode.NotFound;
                message = exception.Message;
            }
            else if (exception is ValidationException)
            {
                statusCode = HttpStatusCode.BadRequest;
                message = exception.Message;
            }
            else if (exception is AppException)
            {
                statusCode = HttpStatusCode.BadRequest;
                message = exception.Message;
            }

            var response = new
            {
                StatusCode = (int)statusCode,
                Message = message,
                Error = exception.GetType().Name // optional, can remove
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var json = JsonSerializer.Serialize(response);

            return context.Response.WriteAsync(json);
        }
    }
}