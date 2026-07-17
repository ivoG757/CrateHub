using Api.Data.Models;

namespace Api.Services.Interaces;

public interface ITokenService
{
    public string CreateToken(int id, string username);
}