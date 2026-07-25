using static Api.Utils.Constants.FileConstants;
using Api.Services.Interfaces;
namespace Api.Services;

public class FileStorage : IFileStorage
{
    private readonly string _basePath;

    public FileStorage(IConfiguration configuration)
    {
        _basePath = configuration["AppSettings:basePath"]!;
        Directory.CreateDirectory(_basePath);
    }

    public Task DeleteAsync(string path)
    {
        throw new NotImplementedException();
    }

    public Stream OpenRead(string path)
    {
        throw new NotImplementedException();
    }

    public async Task<string> SaveAsync(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName);
        var storedName = $"{Guid.NewGuid():N}{extension}";

        var path = Path.Combine(_basePath, storedName);

        await using var fileStream = new FileStream(
            path,
            FileMode.CreateNew,
            FileAccess.Write,
            FileShare.None,
            bufferSize: buffer,
            useAsync: true
        );

        await file.CopyToAsync(fileStream);

        return path;
    }
}