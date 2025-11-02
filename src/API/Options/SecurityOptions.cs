using System.ComponentModel.DataAnnotations;

namespace API.Options;

public class SecurityOptions
{
    public const string SectionName = "Security";

    [Range(1, 1000, ErrorMessage = "Refresh rate limit must be between 1 and 1000 per minute")]
    public int RefreshRateLimitPerMinute { get; set; } = 10;

    [Range(1, 1000, ErrorMessage = "Basket rate limit must be between 1 and 1000 per minute")]
    public int BasketRateLimitPerMinute { get; set; } = 30;
}
