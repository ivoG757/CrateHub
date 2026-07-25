using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Api.Data.Models;

public class FileEntry
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Path { get; set; } = null!;

    public DateTime UploadedAt { get; set; }

    public string StoredName { get; set; } = null!;

    public long Size { get; set; }

    public DateTime ExpiresAt { get; set; }

    public string ContentType { get; set; } = null!;

    public string ShareToken { get; set; } = null!;


    [ForeignKey(nameof(User))]
    public int UserId { get; set; }

    public User User { get; set; } = null!;
}