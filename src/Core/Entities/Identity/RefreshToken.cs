using Core.Common.Entities;
using Core.Entities.Identity;

public class RefreshToken : BaseEntity
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    public string? RevokedByIp { get; set; }
    public string? ReasonRevoked { get; set; }
    public string? ReplacedByToken { get; set; }
    public Guid SessionId { get; set; } = Guid.Empty;
    public string? CreatedByIp { get; set; }
    public string? UserAgent { get; set; }
    public string? DeviceName { get; set; }
    public DateTime? LastUsedAt { get; set; }
    public string UserId { get; set; } = string.Empty;
    public AppUser User { get; set; } = default!;

    public bool IsActive => RevokedAt == null && !IsExpired;
    public bool IsExpired => DateTime.UtcNow >= Expires;
}
