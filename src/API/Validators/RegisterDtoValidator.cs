using Core.Dtos;
using Core.Entities.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace API.Validators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        private readonly UserManager<AppUser> _userManager;
        private static readonly string[] CommonPasswords = new[]
        {
            "password", "123456", "qwerty", "admin", "letmein",
            "welcome", "monkey", "dragon", "master", "sunshine"
        };

        public RegisterDtoValidator(UserManager<AppUser> userManager)
        {
            _userManager = userManager;

            RuleFor(x => x.DisplayName)
                .NotEmpty().WithMessage("Display name is required")
                .Length(2, 50).WithMessage("Display name must be between 2 and 50 characters")
                .Must(NotContainHTML).WithMessage("Display name cannot contain HTML or script tags")
                .Must(BeAlphanumericWithSpaces).WithMessage("Display name can only contain letters, numbers, and spaces");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(255).WithMessage("Email must not exceed 255 characters")
                .Must(NotContainDangerousCharacters).WithMessage("Email contains invalid characters")
                .MustAsync(BeUniqueEmail).WithMessage("Email is already taken");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters")
                .MaximumLength(128).WithMessage("Password must not exceed 128 characters")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches(@"[0-9]").WithMessage("Password must contain at least one digit")
                .Matches(@"[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character")
                .Must(NotBeCommonPassword).WithMessage("Password is too common. Please choose a stronger password");
        }

        private bool NotContainHTML(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return true;

            var htmlPatterns = new[] { "<", ">", "script", "javascript:", "onerror", "onclick" };
            return !htmlPatterns.Any(pattern =>
                text.Contains(pattern, StringComparison.OrdinalIgnoreCase));
        }

        private bool BeAlphanumericWithSpaces(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return true;

            return name.All(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '.' || c == '-' || c == '_');
        }

        private bool NotContainDangerousCharacters(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return true;

            var dangerousPatterns = new[] { "<", ">", "--", "/*", "*/", "script" };
            return !dangerousPatterns.Any(pattern =>
                email.Contains(pattern, StringComparison.OrdinalIgnoreCase));
        }

        private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(email))
                return true;

            var existingUser = await _userManager.FindByEmailAsync(email);
            return existingUser == null;
        }

        private bool NotBeCommonPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return true;

            return !CommonPasswords.Any(commonPwd =>
                password.Contains(commonPwd, StringComparison.OrdinalIgnoreCase));
        }
    }
}
