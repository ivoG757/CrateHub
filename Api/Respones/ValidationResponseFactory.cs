using Microsoft.AspNetCore.Mvc;

namespace Api.Responses;

public static class ValidationResponseFactory
{
    public static IActionResult Create(ActionContext context)
    {
        var errors = context.ModelState
            .Where(e => e.Value?.Errors.Count > 0)
            .ToDictionary(
                e => e.Key,
                e => e.Value!.Errors
                    .Select(error => error.ErrorMessage)
                    .ToArray());

        var isFileTooLarge = context.ModelState
            .Values
            .SelectMany(x => x.Errors)
            .Any(error =>
                error.ErrorMessage.Contains("Multipart body length limit", StringComparison.OrdinalIgnoreCase));
        //TODO: This is a hacky way to check for file size limit errors. 
        //Consider implementing a more robust solution in the future.

        if (isFileTooLarge)
        {
            return new ObjectResult(new ErrorResponse
            {
                Code = "FILE_TOO_LARGE",
                Message = "The uploaded file exceeds the maximum allowed size."
            })
            {
                StatusCode = StatusCodes.Status413PayloadTooLarge
            };
        }
        return new BadRequestObjectResult(new ErrorResponse
        {
            Code = "VALIDATION_ERROR",
            Message = "One or more fields failed validation",
            Errors = errors
        });
    }
}