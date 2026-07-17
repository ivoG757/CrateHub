using System.ComponentModel.DataAnnotations;
using static Api.Constants.Constants.RegisterDtoConstants;
namespace Api.Data.Dtos;

public class RegisterDto
{
    [Required]
    [StringLength(maximumLength: MaxLengthForUsername, MinimumLength = MinLengthForUsername)]
    public string Username { get; set; } = null!;

    [Required]
    [StringLength(maximumLength: MaxLengthForPassword, MinimumLength = MinLengthForPassword)]
    public string Password { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
}