using Api.Exceptions;
using static Microsoft.AspNetCore.Http.StatusCodes;
using Api.Services.Interaces;
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
                _ => "INTERNAL_ERROR"
            };
        }

        public bool IsKnown(Exception exception)
        {
            return exception is AppException;
        }
    }
}