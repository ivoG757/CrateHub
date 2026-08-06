using Api.Exceptions;
using static Microsoft.AspNetCore.Http.StatusCodes;
using Api.Services.Interfaces;
namespace Api.Services.ExceptionHandling
{
    public class ExceptionTranslator : IExceptionTranslator
    {
        public int GetStatusCode(Exception exception)
        {
            return exception switch
            {
                EmailAlreadyExistsException => Status409Conflict,
                UsernameAlreadyExistsException => Status409Conflict,
                InvalidCredentialsException => Status401Unauthorized,
                BadHttpRequestException => Status413PayloadTooLarge,
                FileNotFoundException => Status404NotFound,
                _ => Status500InternalServerError
            };
        }

        public string GetErrorCode(Exception exception)
        {
            return exception switch
            {
                EmailAlreadyExistsException => "EMAIL_ALREADY_EXISTS",
                UsernameAlreadyExistsException => "USERNAME_ALREADY_EXISTS",
                InvalidCredentialsException => "INVALID_CREDENTIALS",
                BadHttpRequestException => "FILE_TOO_LARGE",
                FileNotFoundException => "FILE_NOT_FOUND",
                _ => "INTERNAL_ERROR"
            };
        }

        public string GetErrorMessage(Exception exception)
        {
            if (IsKnown(exception))
            {
                return exception.Message;
            }

            return exception switch
            {
                FileNotFoundException => "The requested file was not found",
                _ => "An unexpected error occurred."
            };
        }
        private bool IsKnown(Exception exception)
        {
            return exception is AppException;
        }
    }
}