using CommandService.Models.Exceptions;
using CommandService.Models.MetaDatas;

namespace CommandService.Middlewares
{
    // class for gerenic exception for the UI handling
    public class ExceptionHandlerMiddleware
    {
        // Fields
        private readonly ILogger<ExceptionHandlerMiddleware> _logger; // for logging
        private readonly RequestDelegate _next; // for the next middleware
        private readonly IHostEnvironment _env; // for

        //constructor
        public ExceptionHandlerMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlerMiddleware> logger,
            IHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _env = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                var errorId = Guid.NewGuid().ToString();
                LogError(errorId, context, exception);
                await HandleExceptionAsync(context, errorId, exception);
            }
        }

        private void LogError(string errorId, HttpContext context, Exception exception)
        {
            var error = new
            {
                ErrorId = errorId,
                Timestamp = DateTime.UtcNow,
                RequestPath = context.Request.Path,
                RequestMethod = context.Request.Method,
                ExceptionType = exception.GetType().Name,
                ExceptionMessage = exception.Message,
                StackTrace = exception.StackTrace,
                InnerException = exception.InnerException?.Message,
                User = context.User?.Identity?.Name ?? "Anonymous",
                AdditionalInfo = GetAdditionalInfo(exception)
            };

            var logLevel = exception switch
            {
                BusinessException => LogLevel.Warning,
                ValidationException => LogLevel.Warning,
                NotFoundException => LogLevel.Information,
                _ => LogLevel.Error
            };

            _logger.Log(logLevel, exception,
                "Error ID: {ErrorId} - Path: {Path} - Method: {Method} - {@error}",
                errorId,
                context.Request.Path,
                context.Request.Method,
                error);
        }

        private object GetAdditionalInfo(Exception exception)
        {
            if (exception is ApiException)
            {
                var type = exception.GetType().Name.Replace("Exception", "");
                return new { Details = $"{type}: {exception.Message}" };
            }
            return new { };
        }

        private async Task HandleExceptionAsync(HttpContext context, string errorId, Exception exception)
        {
            var statusCode = exception switch
            {
                ApiException apiEx => (int)apiEx.StatusCode,
                InvalidOperationException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            // Use the actual exception message instead of generic messages
            var message = _env.IsDevelopment() || exception is ApiException
                ? exception.Message
                : "An unexpected error occurred";

            var errorResponse = ApiResponseBuilder.BuildErrorResponse(
                data: new
                {
                    ErrorId = errorId,
                    Timestamp = DateTime.UtcNow,
                    Details = GetAdditionalInfo(exception)
                },
                statusCode: statusCode,
                message: message
            );

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}

