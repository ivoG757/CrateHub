namespace Api.Exceptions;

public class EmailAlreadyExistsException : AppException
{
    public EmailAlreadyExistsException()
    : base("a user with this email already exists.")
    {
    }
}