namespace Api.Exceptions;

public class FileExpiredException : AppException
{
    public FileExpiredException()
    : base("The requested file has expired.")
    {
    }
}