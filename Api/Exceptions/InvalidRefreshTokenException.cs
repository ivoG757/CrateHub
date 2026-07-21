namespace Api.Exceptions;

public class InvalidRefreshTokenException : AppException
{

    public InvalidRefreshTokenException() : base("Invalid refresh token.") { }
}