using Api.Data.Models;

namespace Api.Services.Interfaces;

public interface ITokenService
{
    public string CreateToken(int id, string username);
    public string CreateRefreshToken();
}