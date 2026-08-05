using Api.Data.Dtos;

namespace Api.Services.Interfaces;

public interface IExceptionTranslator
{
    int GetStatusCode(Exception exception);
    string GetErrorCode(Exception exception);
    string GetErrorMessage(Exception exception);
}