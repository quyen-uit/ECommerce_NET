using Core.Common.Entities;
using Core.Entities.Identity;

public class RefreshToken : BaseEntity
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    public string UserId { get; set; } = string.Empty;
    public AppUser User { get; set; } = default!;

    public bool IsActive => RevokedAt == null && !IsExpired;
    public bool IsExpired => DateTime.UtcNow >= Expires;
}
