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
    public async Task<FileEntry> GetByPublicTokenAsync(string token)
    {
        return await _context.FileEntries.FirstOrDefaultAsync(fe => fe.ShareToken == token);
    }
}