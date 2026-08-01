using System.ComponentModel.DataAnnotations;
using static Api.Utils.Constants.RegisterDtoConstants;
namespace Api.Data.Dtos;

public class RegisterDto
{
    [Required]
    [StringLength(maximumLength: MaxLengthForUsername, MinimumLength = MinLengthForUsername)]
    public string Username { get; init; } = null!;

    [Required]
    [StringLength(maximumLength: MaxLengthForPassword, MinimumLength = MinLengthForPassword)]
    public string Password { get; init; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; init; } = null!;
}