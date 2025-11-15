using Core.Dtos.Colors;
using FluentValidation;

namespace API.Validators
{
    public class CreateColorDtoValidator : AbstractValidator<CreateColorDto>
    {
        public CreateColorDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Color name is required")
                .Length(1, 20).WithMessage("Color name must be between 1 and 20 characters")
                .Must(NotContainHTML).WithMessage("Color name cannot contain HTML or script tags")
                .Must(NotContainDangerousCharacters).WithMessage("Color name contains invalid characters");

            RuleFor(x => x.HexCode)
                .NotEmpty().WithMessage("Hex code is required")
                .Matches(@"^#(?:[0-9a-fA-F]{3}){1,2}$").WithMessage("Hex code must be a valid format (e.g., #FFF or #FFFFFF)")
                .Length(4, 7).WithMessage("Hex code must be 4 or 7 characters including #");
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
