using Core.Dtos;
using FluentValidation;

namespace API.Validators
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(255).WithMessage("Email must not exceed 255 characters")
                .Must(NotContainDangerousCharacters).WithMessage("Email contains invalid characters");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters")
                .MaximumLength(128).WithMessage("Password must not exceed 128 characters");
        }

        private bool NotContainDangerousCharacters(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return true;

            // Prevent common injection attempts
            var dangerousPatterns = new[] { "<", ">", "--", "/*", "*/", "script", "javascript:" };
            return !dangerousPatterns.Any(pattern =>
                email.Contains(pattern, StringComparison.OrdinalIgnoreCase));
        }
    }
}
