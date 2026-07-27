using Api.Data.Dtos;
using Api.Repository.Interfaces;
using Api.Services.Interfaces;
using static Api.Utils.Constants.FileConstants;
using Api.Exceptions;

namespace Api.Services;

public class FileService : IFileService
{
    private readonly IFileRepository _fileRepository;
    private readonly IFileStorage _fileStorage;
    private readonly IShareTokenGenerator _shareTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUrlProvider _urlProvider;

    public FileService(IFileRepository fileRepository,
     IFileStorage fileStorage,
      IShareTokenGenerator shareTokenGenerator,
       IUnitOfWork unitOfWork, IUrlProvider urlProvider)
    {
        _fileRepository = fileRepository;
        _fileStorage = fileStorage;
        _shareTokenGenerator = shareTokenGenerator;
        _unitOfWork = unitOfWork;
        _urlProvider = urlProvider;
    }

    public async Task<FileDto> UploadAsync(IFormFile file, int userId)
    {
        string? path = null;

        FileValidate(file);
        try
        {
            path = await _fileStorage.SaveAsync(file, userId);

            var entry = new FileEntry
            {
                Name = file.FileName,
                StoredName = Path.GetFileName(path),
                Path = path,
                ContentType = file.ContentType,
                Size = file.Length,
                ShareToken = _shareTokenGenerator.Generate(),
                UploadedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(30),
                UserId = userId
            };


            await _fileRepository.AddAsync(entry);

            await _unitOfWork.SaveChangesAsync();


            return new FileDto
            {
                Id = entry.Id,
                FileName = entry.Name,
                FileSize = entry.Size,
                DownloadUrl = _urlProvider.Create(entry.ShareToken),
                ExpiresAt = entry.ExpiresAt,
                UploadedAt = entry.UploadedAt,
                FileType = entry.ContentType
            };
        }
        catch
        {
            if (path != null)
            {
                _fileStorage.Delete(path);
            }

            throw;
        }
    }
    private static void FileValidate(IFormFile file)
    {
        if (file.Length > FileLengthLimit)
        {
            throw new FileTooLargeException();
        }

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(extension) ||
        ForbiddenExtensions.Contains(extension))
        {
            throw new FileFormatNotSupportedException();
        }
    }

    public Task DeleteAsync(FileDto dto, int userId)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<FileDto>> GetFilesAsync(int userId)
    {
        var files = await _fileRepository.GetFilesAsync(userId);

        return files.Select(f => new FileDto
        {
            Id = f.Id,
            FileName = f.Name,
            FileType = f.ContentType,
            FileSize = f.Size,
            DownloadUrl = _urlProvider.Create(f.ShareToken),
            UploadedAt = f.UploadedAt,
            ExpiresAt = f.ExpiresAt

        }).ToArray();
    }
}