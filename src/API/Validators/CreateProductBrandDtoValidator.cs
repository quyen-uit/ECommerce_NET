using Core.Dtos.ProductBrands;
using FluentValidation;

namespace API.Validators
{
    public class CreateProductBrandDtoValidator : AbstractValidator<CreateProductBrandDto>
    {
        public CreateProductBrandDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Brand name is required")
                .Length(1, 100).WithMessage("Brand name must be between 1 and 100 characters")
                .Must(NotContainHTML).WithMessage("Brand name cannot contain HTML or script tags")
                .Must(NotContainDangerousCharacters).WithMessage("Brand name contains invalid characters");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters")
                .Must(NotContainHTML).WithMessage("Description cannot contain HTML or script tags")
                .When(x => !string.IsNullOrWhiteSpace(x.Description));

            RuleFor(x => x.LogoUrl)
                .Must(BeValidUrl).WithMessage("Invalid logo URL format")
                .Must(BeHttpOrHttpsUrl).WithMessage("Logo URL must use HTTP or HTTPS protocol")
                .Must(NotBeJavaScriptUrl).WithMessage("JavaScript URLs are not allowed")
                .Must(NotBeDataUrl).WithMessage("Data URLs are not allowed for security reasons")
                .MaximumLength(500).WithMessage("Logo URL must not exceed 500 characters")
                .When(x => !string.IsNullOrWhiteSpace(x.LogoUrl));
        }

        private bool NotContainHTML(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return true;

            var htmlPatterns = new[]
            {
                "<script", "</script", "<img", "onerror=", "onclick=", "onload=",
                "javascript:", "<iframe", "<object", "<embed", "<svg", "onmouseover="
            };

            return !htmlPatterns.Any(pattern =>
                text.Contains(pattern, StringComparison.OrdinalIgnoreCase));
        }

        private bool NotContainDangerousCharacters(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return true;

            return !text.Contains("--") && !text.Contains("/*") && !text.Contains("*/");
        }

        private bool BeValidUrl(string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return true;

            return Uri.TryCreate(url, UriKind.Absolute, out var uri) &&
                   (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }

        private bool BeHttpOrHttpsUrl(string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return true;

            return url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                   url.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
        }

        private bool NotBeJavaScriptUrl(string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return true;

            return !url.StartsWith("javascript:", StringComparison.OrdinalIgnoreCase);
        }

        private bool NotBeDataUrl(string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return true;

            return !url.StartsWith("data:", StringComparison.OrdinalIgnoreCase);
        }
    }
}
