using Core.Dtos;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using FluentValidation;

namespace API.Validators
{
    public class OrderDtoValidator : AbstractValidator<OrderDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.BasketId)
                .NotEmpty().WithMessage("Basket ID is required")
                .MaximumLength(100).WithMessage("Basket ID must not exceed 100 characters");

            RuleFor(x => x.DeliveryMethod)
                .NotEmpty().WithMessage("Delivery method is required")
                .MustAsync(DeliveryMethodExists).WithMessage("Selected delivery method does not exist");

            RuleFor(x => x.ShipToAddress)
                .NotNull().WithMessage("Shipping address is required")
                .SetValidator(new AddressDtoValidator());
        }

        private async Task<bool> DeliveryMethodExists(Guid deliveryMethodId, CancellationToken cancellationToken)
        {
            var deliveryMethodRepo = _unitOfWork.Repository<DeliveryMethod>();
            var deliveryMethod = await deliveryMethodRepo.GetByIdAsync(deliveryMethodId);
            return deliveryMethod != null;
        }
    }

    public class AddressDtoValidator : AbstractValidator<AddressDto>
    {
        public AddressDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .Length(1, 100).WithMessage("First name must be between 1 and 100 characters")
                .Must(BeValidName).WithMessage("First name contains invalid characters")
                .Must(NotContainHTML).WithMessage("First name cannot contain HTML");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .Length(1, 100).WithMessage("Last name must be between 1 and 100 characters")
                .Must(BeValidName).WithMessage("Last name contains invalid characters")
                .Must(NotContainHTML).WithMessage("Last name cannot contain HTML");

            RuleFor(x => x.HouseNumber)
                .NotEmpty().WithMessage("House number is required")
                .MaximumLength(50).WithMessage("House number must not exceed 50 characters")
                .Must(NotContainHTML).WithMessage("House number cannot contain HTML");

            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("Street is required")
                .MaximumLength(200).WithMessage("Street must not exceed 200 characters")
                .Must(NotContainHTML).WithMessage("Street cannot contain HTML");

            RuleFor(x => x.Ward)
                .NotEmpty().WithMessage("Ward is required")
                .MaximumLength(100).WithMessage("Ward must not exceed 100 characters")
                .Must(NotContainHTML).WithMessage("Ward cannot contain HTML");

            RuleFor(x => x.District)
                .NotEmpty().WithMessage("District is required")
                .MaximumLength(100).WithMessage("District must not exceed 100 characters")
                .Must(NotContainHTML).WithMessage("District cannot contain HTML");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required")
                .MaximumLength(100).WithMessage("City must not exceed 100 characters")
                .Must(NotContainHTML).WithMessage("City cannot contain HTML");

            RuleFor(x => x.ZipCode)
                .GreaterThan(0).WithMessage("Zip code must be greater than 0")
                .LessThan(1000000).WithMessage("Zip code must be a valid postal code");
        }

        private bool BeValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return true;

            // Allow letters, spaces, hyphens, apostrophes (common in names)
            return name.All(c => char.IsLetter(c) || char.IsWhiteSpace(c) || c == '-' || c == '\'');
        }

        private bool NotContainHTML(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return true;

            var htmlPatterns = new[] { "<", ">", "script", "javascript:", "onerror", "onclick" };
            return !htmlPatterns.Any(pattern =>
                text.Contains(pattern, StringComparison.OrdinalIgnoreCase));
        }
    }
}
