using Core.Dtos.Images;
using FluentValidation;

namespace API.Validators
{
    public class CreateImageDtoValidator : AbstractValidator<CreateImageDto>
    {
        public CreateImageDtoValidator()
        {
            RuleFor(x => x.Url)
                .NotEmpty().WithMessage("Image URL is required")
                .MaximumLength(500).WithMessage("Image URL must not exceed 500 characters")
                .Must(BeValidUrl).WithMessage("Invalid image URL format")
                .Must(BeHttpOrHttpsUrl).WithMessage("Image URL must use HTTP or HTTPS protocol")
                .Must(NotBeJavaScriptUrl).WithMessage("JavaScript URLs are not allowed")
                .Must(NotBeDataUrl).WithMessage("Data URLs are not allowed for security reasons");

            RuleFor(x => x.Order)
                .GreaterThanOrEqualTo(0).WithMessage("Order must be greater than or equal to 0")
                .LessThan(1000).WithMessage("Order must be less than 1000");

            RuleFor(x => x.Type)
                .IsInEnum().WithMessage("Invalid image type");
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

    public class CreateListImageDtoValidator : AbstractValidator<CreateListImageDto>
    {
        public CreateListImageDtoValidator()
        {
            RuleFor(x => x.ReferenceId)
                .NotEmpty().WithMessage("Reference ID is required");

            RuleFor(x => x.CreateImageDtos)
                .NotNull().WithMessage("Image list cannot be null")
                .Must(x => x.Count <= 50).WithMessage("Maximum 50 images allowed per reference");

            RuleForEach(x => x.CreateImageDtos)
                .SetValidator(new CreateImageDtoValidator());
        }
    }
}
