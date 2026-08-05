using Api.Responses;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Diagnostics;

namespace Api.Services.ExceptionHandling;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var translator = context.RequestServices
            .GetRequiredService<IExceptionTranslator>();

        _logger.LogError(
            exception,
            "Unhandled exception occurred while processing {Method} {Path}",
            context.Request.Method,
            context.Request.Path);


        var response = new ErrorResponse
        {
            Code = translator.GetErrorCode(exception),
            Message = translator.GetErrorMessage(exception)
        };


        context.Response.StatusCode =
            translator.GetStatusCode(exception);

        context.Response.ContentType = "application/json";


        await context.Response.WriteAsJsonAsync(
            response,
            cancellationToken);


        return true;
    }
}