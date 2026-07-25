namespace Api.Exceptions;

public class UserNotFoundException : AppException
{
    public UserNotFoundException()
    : base("The user could not be found.")
    { }
}