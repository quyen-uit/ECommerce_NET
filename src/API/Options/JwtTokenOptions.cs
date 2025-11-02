using System.ComponentModel.DataAnnotations;

namespace API.Options;

public class JwtTokenOptions
{
    public const string SectionName = "Token";

    [Required(ErrorMessage = "Token Key is required")]
    [MinLength(32, ErrorMessage = "Token Key must be at least 32 characters long")]
    public string Key { get; set; } = string.Empty;

    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;

    [Range(1, 1440, ErrorMessage = "Access token expiration must be between 1 and 1440 minutes")]
    public int AccessTokenExpiration { get; set; } = 15;

    [Range(1, 365, ErrorMessage = "Refresh token expiration must be between 1 and 365 days")]
    public int RefreshTokenExpirationDays { get; set; } = 7;

    [Range(1, 100, ErrorMessage = "Max sessions per user must be between 1 and 100")]
    public int MaxSessionsPerUser { get; set; } = 3;
}
