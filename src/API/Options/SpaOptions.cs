using System.ComponentModel.DataAnnotations;

namespace API.Options;

public class SpaOptions
{
    public const string SectionName = "Spa";

    [Required(ErrorMessage = "SPA Origin is required")]
    [Url(ErrorMessage = "SPA Origin must be a valid URL")]
    public string Origin { get; set; } = string.Empty;
}
