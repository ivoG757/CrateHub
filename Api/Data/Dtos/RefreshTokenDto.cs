using System.ComponentModel.DataAnnotations;

namespace Api.Data.Dtos;

public class RefreshTokenDto
{
    [Required]
    public string Token { get; set; } = null!;
}