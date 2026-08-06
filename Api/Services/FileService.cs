using Api.Data.Dtos;
using Api.Repository.Interfaces;
using Api.Services.Interfaces;
using static Api.Utils.Constants;
using Api.Exceptions;

namespace Api.Services;

public class FileService : IFileService
{
    private readonly IFileRepository _fileRepository;
    private readonly IFileStorage _fileStorage;
    private readonly IShareTokenGenerator _shareTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<FileService> _logger;
    public FileService(IFileRepository fileRepository,
     IFileStorage fileStorage,
      IShareTokenGenerator shareTokenGenerator,
       IUnitOfWork unitOfWork,
       ILogger<FileService> logger)
    {
        _fileRepository = fileRepository;
        _fileStorage = fileStorage;
        _shareTokenGenerator = shareTokenGenerator;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<FileDto> UploadAsync(IFormFile file, int userId)
    {
        _logger.LogInformation("Uploading file for user {UserId}", userId);

        string? path = null;

        FileValidate(file);
        try
        {
            path = await _fileStorage.SaveAsync(file, userId);

            var now = DateTime.UtcNow;

            var entry = new FileEntry
            {
                Name = file.FileName,
                StoredName = Path.GetFileName(path),
                Path = path,
                ContentType = file.ContentType,
                Size = file.Length,
                ShareToken = _shareTokenGenerator.Generate(),
                UploadedAt = now,
                ExpiresAt = now.AddMinutes(30),
                UserId = userId
            };


            await _fileRepository.AddAsync(entry);

            await _unitOfWork.SaveChangesAsync();

            var fileDto = MapToFileDto(entry);

            _logger.LogInformation("User {userId} successfully uploaded file with id: {Id}.", userId, fileDto.Id);

            return fileDto;
        }
        catch (Exception ex)
        {
            if (path != null)
            {
                _fileStorage.Delete(path);
            }

            _logger.LogError(
                ex,
                "Failed to upload file for user {UserId}.",
                userId);


            throw;
        }
    }
    private void FileValidate(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            _logger.LogWarning("Could not upload file: the file could not be found or it doesn't exist.");
            throw new FileFormatNotSupportedException();
        }

        if (file.Length > Files.MaxSize)
        {
            _logger.LogWarning("Could not upload file: the file is too large.");
            throw new FileTooLargeException();
        }

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(extension) ||
        Files.ForbiddenExtensions.Contains(extension))
        {
            _logger.LogWarning("Could not upload file: selected file's format is not supported.");
            throw new FileFormatNotSupportedException();
        }
    }

    public async Task DeleteAsync(int fileId, int userId)
    {
        var fileToDelete = await _fileRepository.GetFileByIdAsync(fileId, userId);

        if (fileToDelete == null)
        {
            _logger.LogWarning("Could not delete file: the file could not be found or it doesn't exist.");
            throw new FileNotFoundException("File not found");
        }
        try
        {
            _fileStorage.Delete(fileToDelete.Path);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to delete file from storage for user {UserId}. FileId: {FileId}",
                userId,
                fileId);

            throw;
        }

        _fileRepository.Delete(fileToDelete);

        try
        {
            await _unitOfWork.SaveChangesAsync();
        }

        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to remove file database entry. FileEntryId: {FileId}",
                fileId);

            throw;
        }

        _logger.LogInformation(
            "User {UserId} deleted file {FileId} ({FileName})",
            userId,
            fileToDelete.Id,
            fileToDelete.Name);
    }

    public async Task<ICollection<FileDto>> GetFilesAsync(int userId)
    {
        var files = await _fileRepository.GetFilesAsync(userId);

        return files.Select(f => MapToFileDto(f)).ToArray();
    }

    public async Task<FileDto?> GetShareInfoAsync(string token)
    {
        var fileEntry = await _fileRepository.GetByPublicTokenAsync(token);

        if (fileEntry == null)
        {
            _logger.LogWarning("Could not retrieve share info: the file could not be found or it doesn't exist.");
            throw new FileNotFoundException("File not found");
        }

        if (fileEntry.ExpiresAt < DateTime.UtcNow)
        {
            _logger.LogWarning("Could not retrieve share info: the file has expired.");
            throw new FileExpiredException();
        }

        var fileDto = MapToFileDto(fileEntry);

        return fileDto;
    }

    public async Task<DownloadFileDto> DownloadAsync(string token)
    {
        var fileEntry = await _fileRepository.GetByPublicTokenAsync(token);

        if (fileEntry == null)
        {
            _logger.LogWarning("Could not download file: the file could not be found or it doesn't exist.");
            throw new FileNotFoundException("File not found");
        }

        if (fileEntry.ExpiresAt < DateTime.UtcNow)
        {
            _logger.LogWarning("Could not download file: the file has expired.");
            throw new FileExpiredException();
        }

        if (!_fileStorage.Exists(fileEntry.Path))
        {
            _logger.LogWarning("Could not download file: the file could not be found or it doesn't exist.");
            throw new FileNotFoundException("File not found");
        }

        var stream = _fileStorage.GetStream(fileEntry.Path);

        var downloadFileDto = new DownloadFileDto
        {
            Stream = stream,
            ContentType = fileEntry.ContentType,
            FileName = fileEntry.Name
        };

        _logger.LogInformation(
            "File {FileId} was streamed successfully. Filename: {FileName}",
            fileEntry.Id,
            fileEntry.Name);

        return downloadFileDto;
    }
    private static FileDto MapToFileDto(FileEntry entry)
    {
        return new FileDto
        {
            Id = entry.Id,
            FileName = entry.Name,
            FileType = entry.ContentType,
            FileSize = entry.Size,
            ShareToken = entry.ShareToken,
            UploadedAt = entry.UploadedAt,
            ExpiresAt = entry.ExpiresAt
        };
    }
}