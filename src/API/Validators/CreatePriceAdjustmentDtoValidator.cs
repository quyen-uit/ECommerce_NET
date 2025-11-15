using Core.Dtos.PriceAdjustments;
using Core.Entities;
using Core.Interfaces;
using FluentValidation;

namespace API.Validators
{
    public class CreatePriceAdjustmentDtoValidator : AbstractValidator<CreatePriceAdjustmentDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePriceAdjustmentDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.ProductSkuId)
                .NotEmpty().WithMessage("Product SKU is required")
                .MustAsync(ProductSkuExists).WithMessage("Selected product SKU does not exist or has been deleted");

            RuleFor(x => x.SalePrice)
                .GreaterThan(0).WithMessage("Sale price must be greater than 0")
                .LessThan(1000000).WithMessage("Sale price must be less than 1,000,000");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required")
                .Must(BeValidDate).WithMessage("Start date must be a valid date");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End date is required")
                .Must(BeValidDate).WithMessage("End date must be a valid date")
                .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date");

            RuleFor(x => x.PriceAdjustmentItems)
                .Must(x => x == null || x.Count <= 100).WithMessage("Maximum 100 price adjustment items allowed");
        }

        private bool BeValidDate(DateTime date)
        {
            return date != default && date > DateTime.MinValue && date < DateTime.MaxValue;
        }

        private async Task<bool> ProductSkuExists(Guid productSkuId, CancellationToken cancellationToken)
        {
            var productSkuRepo = _unitOfWork.Repository<ProductSku>();
            var productSku = await productSkuRepo.GetByIdAsync(productSkuId);
            return productSku != null && !productSku.IsDeleted;
        }
    }
}
