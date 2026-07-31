using Api.Responses;
using Api.Services.Interfaces;

namespace Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IExceptionTranslator translator)
    {
        try
        {
            await _next(context);
        }

        catch (Exception ex)
        {
            var response = new ErrorResponse
            {
                Code = translator.GetErrorCode(ex),
                Message = translator.IsKnown(ex) ? ex.Message : "Internal server error."
            };
            _logger.LogError(
                ex,
                "Unhandled exception while processing {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            context.Response.StatusCode = translator.GetStatusCode(ex);
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}