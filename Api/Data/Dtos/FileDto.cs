namespace Api.Data.Dtos;

public class FileDto
{
    public int Id { get; set; }

    public string FileName { get; set; } = null!;

    public string FileType { get; set; } = null!;

    public long FileSize { get; set; }
    public string DownloadUrl { get; set; } = null!;

    public DateTime UploadedAt { get; set; }

    public DateTime ExpiresAt { get; set; }
}