namespace Api.Data.Dtos;

public sealed class AuthResponseDto
{
    public string AccessToken { get; init; } = null!;
    public string RefreshToken { get; init; } = null!;
}