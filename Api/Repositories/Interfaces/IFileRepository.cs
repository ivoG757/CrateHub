namespace Api.Repository.Interfaces;

public interface IFileRepository
{
    public Task AddAsync(FileEntry file);
    public Task<FileEntry> GetByPublicTokenAsync(string token);
}