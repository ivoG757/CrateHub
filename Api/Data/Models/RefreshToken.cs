using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;

namespace Api.Data.Models;

public sealed class RefreshToken
{
    public int Id { get; set; }

    public string Token { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime ExpiresAt { get; set; }

    public int UserId { get; set; }

    public User User { get; set; } = null!;
}