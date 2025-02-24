using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace EmployeeManagementSystem.API.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var activityId = Guid.NewGuid();
            context.Items["ActivityId"] = activityId;
            var stopwatch = Stopwatch.StartNew();
            try
            {
                using (_logger.BeginScope(new Dictionary<string, object> { { "ActivityId", activityId } }))
                {
                    _logger.LogInformation("Request Started: {ActivityId}, {Method} {Path} {QueryString}",
                        activityId, context.Request.Method, context.Request.Path, context.Request.QueryString);
                    await _next(context);

                    stopwatch.Stop();
                    _logger.LogInformation("Request Completed: {ActivityId}, Status: {StatusCode}, Duration: {ElapsedMs}ms",
                      activityId, context.Response.StatusCode, stopwatch.ElapsedMilliseconds);

                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                await HandleExceptionAsync(context, ex);
                _logger.LogError(ex, "Request Failed: {ActivityId}, Status: {StatusCode}, Duration: {ElapsedMs}ms",
                       activityId, context.Response.StatusCode, stopwatch.ElapsedMilliseconds);

            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            HttpStatusCode statusCode;
            string message;
            string stackTrace = null;

            switch (exception)
            {
                case ArgumentException argEx:
                    statusCode = HttpStatusCode.BadRequest;
                    message = argEx.Message;
                    break;
                case KeyNotFoundException keyEx:
                    statusCode = HttpStatusCode.NotFound;
                    message = keyEx.Message;
                    break;
                
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    message = "An unexpected error occurred.";
                    stackTrace = exception.StackTrace;
                    _logger.LogError(exception, message);
                    break;
            }

            context.Response.StatusCode = (int)statusCode;

            var errorResponse = new ProblemDetails()
            {
                Status = (int)statusCode,
                Title = message,
                Detail = stackTrace,
                Type = statusCode.ToString(),
            };

            var json = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(json);
        }
    }
}
