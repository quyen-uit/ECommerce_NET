using Core.Dtos;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using FluentValidation;

namespace API.Validators
{
    public class CustomerBasketDtoValidator : AbstractValidator<CustomerBasketDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerBasketDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Basket ID is required")
                .MaximumLength(100).WithMessage("Basket ID must not exceed 100 characters");

            RuleFor(x => x.Items)
                .NotNull().WithMessage("Items list cannot be null")
                .Must(x => x.Count <= 50).WithMessage("Maximum 50 items allowed in basket");

            RuleForEach(x => x.Items)
                .SetValidator(new BasketItemDtoValidator());

            RuleFor(x => x.DeliveryMethodId)
                .MustAsync(DeliveryMethodExistsIfProvided!)
                .WithMessage("Selected delivery method does not exist")
                .When(x => x.DeliveryMethodId.HasValue);

            RuleFor(x => x.ShippingPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Shipping price must be greater than or equal to 0");
        }

        private async Task<bool> DeliveryMethodExistsIfProvided(Guid? deliveryMethodId, CancellationToken cancellationToken)
        {
            if (!deliveryMethodId.HasValue)
                return true;

            var deliveryMethodRepo = _unitOfWork.Repository<DeliveryMethod>();
            var deliveryMethod = await deliveryMethodRepo.GetByIdAsync(deliveryMethodId.Value);
            return deliveryMethod != null;
        }
    }

    public class BasketItemDtoValidator : AbstractValidator<BasketItemDto>
    {
        public BasketItemDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Product ID is required");

            RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage("Product name is required")
                .MaximumLength(200).WithMessage("Product name must not exceed 200 characters")
                .Must(NotContainHTML).WithMessage("Product name cannot contain HTML");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0")
                .LessThan(1000000).WithMessage("Price must be less than 1,000,000");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be at least 1")
                .LessThanOrEqualTo(999).WithMessage("Quantity cannot exceed 999");

            RuleFor(x => x.PhotoUrl)
                .NotEmpty().WithMessage("Photo URL is required")
                .MaximumLength(500).WithMessage("Photo URL must not exceed 500 characters")
                .Must(BeValidUrl).WithMessage("Invalid photo URL format");

            RuleFor(x => x.Brand)
                .NotEmpty().WithMessage("Brand is required")
                .MaximumLength(100).WithMessage("Brand must not exceed 100 characters")
                .Must(NotContainHTML).WithMessage("Brand cannot contain HTML");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Type is required")
                .MaximumLength(100).WithMessage("Type must not exceed 100 characters")
                .Must(NotContainHTML).WithMessage("Type cannot contain HTML");
        }

        private bool NotContainHTML(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return true;

            var htmlPatterns = new[] { "<", ">", "script", "javascript:", "onerror", "onclick" };
            return !htmlPatterns.Any(pattern =>
                text.Contains(pattern, StringComparison.OrdinalIgnoreCase));
        }

        private bool BeValidUrl(string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return true;

            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }
    }
}
