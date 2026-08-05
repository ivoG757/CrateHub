using Api.Responses;
using Api.Services.Interfaces;

namespace Api.Middleware;

[Obsolete("This middleware is deprecated. Use the GlobalExceptionHandler instead.", false)]
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

            var message = translator.GetErrorMessage(ex);
            var code = translator.GetErrorCode(ex);

            var response = new ErrorResponse
            {
                Code = code,
                Message = message
            };

            context.Response.StatusCode = translator.GetStatusCode(ex);
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}