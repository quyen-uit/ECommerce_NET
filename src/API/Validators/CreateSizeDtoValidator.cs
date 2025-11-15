using Core.Dtos.Sizes;
using FluentValidation;

namespace API.Validators
{
    public class CreateSizeDtoValidator : AbstractValidator<CreateSizeDto>
    {
        public CreateSizeDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Size name is required")
                .Length(1, 50).WithMessage("Size name must be between 1 and 50 characters")
                .Must(NotContainHTML).WithMessage("Size name cannot contain HTML or script tags")
                .Must(NotContainDangerousCharacters).WithMessage("Size name contains invalid characters");

            RuleFor(x => x.SizeType)
                .NotEmpty().WithMessage("Size type is required")
                .MaximumLength(50).WithMessage("Size type must not exceed 50 characters")
                .Must(NotContainHTML).WithMessage("Size type cannot contain HTML or script tags");

            RuleFor(x => x.SortOrder)
                .GreaterThanOrEqualTo(0).WithMessage("Sort order must be greater than or equal to 0")
                .LessThan(10000).WithMessage("Sort order must be less than 10000");
        }

        private bool NotContainHTML(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return true;

            var htmlPatterns = new[] { "<", ">", "script", "javascript:", "onerror", "onclick" };
            return !htmlPatterns.Any(pattern =>
                text.Contains(pattern, StringComparison.OrdinalIgnoreCase));
        }

        private bool NotContainDangerousCharacters(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return true;

            return !text.Contains("--") && !text.Contains("/*") && !text.Contains("*/");
        }
    }
}
