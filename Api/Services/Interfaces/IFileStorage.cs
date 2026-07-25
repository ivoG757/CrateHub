using Api.Data.Dtos;

namespace Api.Services.Interfaces;

public interface IFileStorage
{
    Task<string> SaveAsync(IFormFile file, int userId);
    Task DeleteAsync(string path);
    Stream OpenRead(string path);
}