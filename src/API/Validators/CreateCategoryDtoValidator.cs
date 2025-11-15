using Core.Dtos.Categories;
using Core.Entities;
using Core.Interfaces;
using FluentValidation;

namespace API.Validators
{
    public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCategoryDtoValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required")
                .Length(1, 100).WithMessage("Category name must be between 1 and 100 characters")
                .Must(NotContainHTML).WithMessage("Category name cannot contain HTML or script tags")
                .Must(NotContainDangerousCharacters).WithMessage("Category name contains invalid characters");

            RuleFor(x => x.ParentId)
                .MustAsync(ParentCategoryExistsIfProvided)
                .WithMessage("Selected parent category does not exist or has been deleted")
                .MustAsync(NotCreateCircularReference)
                .WithMessage("Cannot set parent category that would create a circular reference")
                .When(x => x.ParentId.HasValue && x.ParentId.Value != Guid.Empty);

            RuleFor(x => x.Order)
                .GreaterThanOrEqualTo(0).WithMessage("Order must be greater than or equal to 0")
                .LessThan(1000).WithMessage("Order must be less than 1000");
        }

        private bool NotContainHTML(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return true;

            var htmlPatterns = new[]
            {
                "<script", "</script", "<img", "onerror=", "onclick=", "onload=",
                "javascript:", "<iframe", "<object", "<embed", "<svg"
            };

            return !htmlPatterns.Any(pattern =>
                text.Contains(pattern, StringComparison.OrdinalIgnoreCase));
        }

        private bool NotContainDangerousCharacters(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return true;

            return !text.Contains("--") && !text.Contains("/*") && !text.Contains("*/");
        }

        private async Task<bool> ParentCategoryExistsIfProvided(Guid? parentId, CancellationToken cancellationToken)
        {
            if (!parentId.HasValue || parentId.Value == Guid.Empty)
                return true;

            var categoryRepo = _unitOfWork.Repository<Category>();
            var parentCategory = await categoryRepo.GetByIdAsync(parentId.Value);
            return parentCategory != null && !parentCategory.IsDeleted;
        }

        private async Task<bool> NotCreateCircularReference(CreateCategoryDto dto, Guid? parentId, CancellationToken cancellationToken)
        {
            // If creating a new category (no Id), no circular reference possible
            if (!dto.Id.HasValue || dto.Id.Value == Guid.Empty)
                return true;

            // If no parent, no circular reference
            if (!parentId.HasValue || parentId.Value == Guid.Empty)
                return true;

            // Cannot set self as parent
            if (dto.Id.Value == parentId.Value)
                return false;

            // Check if parentId is a descendant of current category
            var categoryRepo = _unitOfWork.Repository<Category>();
            var currentParentId = parentId.Value;

            // Traverse up the parent chain (max 10 levels to prevent infinite loops)
            for (int i = 0; i < 10; i++)
            {
                var parent = await categoryRepo.GetByIdAsync(currentParentId);
                if (parent == null)
                    break;

                // If we reach the original category, it's a circular reference
                if (parent.Id == dto.Id.Value)
                    return false;

                // Move to next parent
                if (!parent.ParentId.HasValue || parent.ParentId.Value == Guid.Empty)
                    break;

                currentParentId = parent.ParentId.Value;
            }

            return true;
        }
    }
}
