using Api.Data.Models;

namespace Api.Repository.Interfaces;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken refreshToken);
    Task<RefreshToken?> GetByTokenAsync(string token);
    void Delete(RefreshToken refreshToken);
}