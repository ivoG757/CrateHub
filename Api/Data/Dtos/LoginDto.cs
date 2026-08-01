using System.ComponentModel.DataAnnotations;

namespace Api.Data.Dtos;

public class LoginDto
{
    [Required]
    public string Username { get; init; } = null!;

    [Required]
    public string Password { get; init; } = null!;
}