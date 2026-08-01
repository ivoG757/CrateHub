namespace Api.Repository.Interfaces;

public interface IFileRepository
{
    public Task AddAsync(FileEntry file);

    public Task<FileEntry?> GetByPublicTokenAsync(string token);

    public Task<ICollection<FileEntry>> GetFilesAsync(int userId);

    public Task<ICollection<FileEntry>> GetExpiredFileEntriesAsync();

    public void Delete(FileEntry file);

    public Task<FileEntry?> GetFileByIdAsync(int fileId, int userId);
}