namespace Api.Exceptions;

public class FileTooLargeException : AppException
{
    public FileTooLargeException()
    : base("The selected file is too large.")
    { }
}