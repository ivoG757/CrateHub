namespace Api.Exceptions;

public class FileFormatNotSupportedException : AppException
{
    public FileFormatNotSupportedException()
    : base("The current file format is not supprted.")
    {
    }
}