using System.ComponentModel.DataAnnotations;
using static Api.Utils.Constants.Validation;
namespace Api.Data.Dtos;

public class RegisterDto
{
    [Required]
    [StringLength(maximumLength: UsernameMaxLength, MinimumLength = UsernameMinLength)]
    public string Username { get; init; } = null!;

    [Required]
    [StringLength(maximumLength: PasswordMaxLength, MinimumLength = PasswordMinLength)]
    public string Password { get; init; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; init; } = null!;
}