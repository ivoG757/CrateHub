using Api.Data.Dtos;

namespace Api.Services.Interaces;

public interface IAuthService
{
    public Task<string> RegisterAsync(RegisterDto dto);
    public Task<string> LoginAsync(LoginDto dto);
}