using Core.Dtos.Products;
using Core.Entities;
using Core.Interfaces;
using FluentValidation;

namespace API.Validators
{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required")
                .Length(1, 100).WithMessage("Product name must be between 1 and 100 characters")
                .Must(NotContainHTML).WithMessage("Product name cannot contain HTML or script tags")
                .Must(NotContainDangerousCharacters).WithMessage("Product name contains invalid characters");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Product description is required")
                .MaximumLength(1000).WithMessage("Product description must not exceed 1000 characters")
                .Must(NotContainHTML).WithMessage("Product description cannot contain HTML or script tags");

            RuleFor(x => x.PhotoUrl)
                .Must(BeValidUrl).WithMessage("Invalid photo URL format")
                .Must(BeHttpOrHttpsUrl).WithMessage("Photo URL must use HTTP or HTTPS protocol")
                .Must(NotBeJavaScriptUrl).WithMessage("JavaScript URLs are not allowed")
                .Must(NotBeDataUrl).WithMessage("Data URLs are not allowed for security reasons")
                .MaximumLength(500).WithMessage("Photo URL must not exceed 500 characters")
                .When(x => !string.IsNullOrWhiteSpace(x.PhotoUrl));

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Category is required")
                .MustAsync(CategoryExists).WithMessage("Selected category does not exist or has been deleted");

            RuleFor(x => x.ProductBrandId)
                .NotEmpty().WithMessage("Product brand is required")
                .MustAsync(BrandExists).WithMessage("Selected brand does not exist or has been deleted");

            RuleFor(x => x.Properties)
                .Must(x => x == null || x.Count <= 50).WithMessage("Maximum 50 properties allowed per product")
                .ForEach(property =>
                {
                    property.ChildRules(p =>
                    {
                        p.RuleFor(x => x.Key)
                            .NotEmpty().WithMessage("Property key is required")
                            .MaximumLength(50).WithMessage("Property key must not exceed 50 characters")
                            .Must(NotContainHTML).WithMessage("Property key cannot contain HTML");

                        p.RuleFor(x => x.Value)
                            .NotEmpty().WithMessage("Property value is required")
                            .MaximumLength(100).WithMessage("Property value must not exceed 100 characters")
                            .Must(NotContainHTML).WithMessage("Property value cannot contain HTML");
                    });
                });
        }

        private bool NotContainHTML(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return true;

            // Check for common HTML/XSS patterns
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

            // Prevent common injection patterns
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

        private async Task<bool> CategoryExists(Guid categoryId, CancellationToken cancellationToken)
        {
            var categoryRepo = _unitOfWork.Repository<Category>();
            var category = await categoryRepo.GetByIdAsync(categoryId);
            return category != null && !category.IsDeleted;
        }

        private async Task<bool> BrandExists(Guid brandId, CancellationToken cancellationToken)
        {
            var brandRepo = _unitOfWork.Repository<ProductBrand>();
            var brand = await brandRepo.GetByIdAsync(brandId);
            return brand != null && !brand.IsDeleted;
        }
    }
}
