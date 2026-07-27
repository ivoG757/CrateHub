using Api.Data.Dtos;

namespace Api.Services.Interfaces;

public interface IFileStorage
{
    Task<string> SaveAsync(IFormFile file, int userId);
    void Delete(string path);
    Stream OpenRead(string path);
}