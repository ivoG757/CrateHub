using Api.Data.Dtos;
using Api.Responses;

namespace Api.Services.Interfaces;

public interface IAuthService
{
    public Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
    public Task<AuthResponseDto> LoginAsync(LoginDto dto);
    public Task<AuthResponseDto> RefreshAsync(RefreshTokenDto dto);
}