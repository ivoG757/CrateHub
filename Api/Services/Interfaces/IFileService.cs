using Api.Data.Dtos;
namespace Api.Services.Interfaces;

public interface IFileService
{
    public Task<FileDto> UploadAsync(IFormFile file, int userId);
    public Task<ICollection<FileDto>> GetFilesAsync(int userId);
    public Task DeleteAsync(int fileId, int userId);
}