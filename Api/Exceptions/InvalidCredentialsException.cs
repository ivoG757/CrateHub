namespace Api.Exceptions;

public class InvalidCredentialsException : AppException
{

    public InvalidCredentialsException() : base("Password or username is incorrect.")
    {
    }
}