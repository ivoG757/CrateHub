namespace Api.Exceptions;

public class InvalidCredentialsException : AppException
{

    public InvalidCredentialsException() : base("Invalid credentials.")
    {
    }
}