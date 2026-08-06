using System.ComponentModel.DataAnnotations;
using static Api.Utils.Constants.Validation;
namespace Api.Data.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(UsernameMaxLength, MinimumLength = UsernameMinLength)]
    public string Username { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public ICollection<FileEntry> FileEntries { get; set; } = new List<FileEntry>();
}