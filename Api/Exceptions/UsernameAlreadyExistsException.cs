namespace Api.Exceptions;

public class UsernameAlreadyExistsException : AppException
{
    public UsernameAlreadyExistsException()
    : base("a user with this username already exists.")
    { }
}