using Api.Data.Dtos;

namespace Api.Services.Interaces;

public interface IExceptionTranslator
{
    int GetStatusCode(Exception exception);
    string GetErrorCode(Exception exception);
    bool IsKnown(Exception exception);
}