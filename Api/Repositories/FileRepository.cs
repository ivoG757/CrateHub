using Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Repository.Interfaces;

public class FileRepository : IFileRepository
{
    private readonly AppDbContext _context;
    public FileRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(FileEntry file)
    {
        await _context.FileEntries.AddAsync(file);
    }

    public void Delete(FileEntry file)
    {
        _context.FileEntries.Remove(file);
    }

    public async Task<FileEntry?> GetByPublicTokenAsync(string token)
    {
        return await _context.FileEntries.FirstOrDefaultAsync(fe => fe.ShareToken == token);
    }

    public async Task<ICollection<FileEntry>> GetExpiredFileEntriesAsync()
    {
        return await _context.FileEntries.AsNoTracking().Where(fe => fe.ExpiresAt < DateTime.UtcNow).ToArrayAsync();
    }

    public async Task<FileEntry?> GetFileByIdAsync(int id, int userId)
    {
        return await _context.FileEntries
            .FirstOrDefaultAsync(f =>
                f.Id == id &&
                f.UserId == userId);
    }

    public async Task<ICollection<FileEntry>> GetFilesAsync(int userId)
    {
        return await _context.FileEntries.Where(fe => fe.UserId == userId).ToArrayAsync();
    }
}