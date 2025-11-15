using Core.Dtos.ProductSkus;
using Core.Entities;
using Core.Interfaces;
using FluentValidation;

namespace API.Validators
{
    public class CreateProductSkuDtoValidator : AbstractValidator<CreateProductSkuDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductSkuDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.SkuCode)
                .NotEmpty().WithMessage("SKU code is required")
                .Length(1, 50).WithMessage("SKU code must be between 1 and 50 characters")
                .Must(BeAlphanumericWithDashesUnderscores).WithMessage("SKU code can only contain letters, numbers, dashes, and underscores")
                .Must(NotContainHTML).WithMessage("SKU code cannot contain HTML or script tags");

            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product is required")
                .MustAsync(ProductExists).WithMessage("Selected product does not exist or has been deleted");

            RuleFor(x => x.ColorId)
                .NotEmpty().WithMessage("Color is required")
                .MustAsync(ColorExists).WithMessage("Selected color does not exist or has been deleted");

            RuleFor(x => x.SizeId)
                .NotEmpty().WithMessage("Size is required")
                .MustAsync(SizeExists).WithMessage("Selected size does not exist or has been deleted");
        }

        private bool BeAlphanumericWithDashesUnderscores(string skuCode)
        {
            if (string.IsNullOrWhiteSpace(skuCode))
                return true;

            return skuCode.All(c => char.IsLetterOrDigit(c) || c == '-' || c == '_');
        }

        private bool NotContainHTML(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return true;

            var htmlPatterns = new[] { "<", ">", "script", "javascript:" };
            return !htmlPatterns.Any(pattern =>
                text.Contains(pattern, StringComparison.OrdinalIgnoreCase));
        }

        private async Task<bool> ProductExists(Guid productId, CancellationToken cancellationToken)
        {
            var productRepo = _unitOfWork.Repository<Product>();
            var product = await productRepo.GetByIdAsync(productId);
            return product != null && !product.IsDeleted;
        }

        private async Task<bool> ColorExists(Guid colorId, CancellationToken cancellationToken)
        {
            var colorRepo = _unitOfWork.Repository<Color>();
            var color = await colorRepo.GetByIdAsync(colorId);
            return color != null && !color.IsDeleted;
        }

        private async Task<bool> SizeExists(Guid sizeId, CancellationToken cancellationToken)
        {
            var sizeRepo = _unitOfWork.Repository<Size>();
            var size = await sizeRepo.GetByIdAsync(sizeId);
            return size != null && !size.IsDeleted;
        }
    }
}
