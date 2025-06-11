using Core.Dtos;
using Core.Specifications.Categories;

namespace Core.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<IReadOnlyList<CategoryDto>> GetAllCategoriesAsync(CategorySpecParams specParams);
        Task<CategoryDto> GetCategoryByIdAsync(long id);
        Task<CategoryDto> AddCategoryAsync(CreateCategoryDto dto);
        Task<IReadOnlyList<CategoryDto>> AddRangeCategoryAsync(
            IReadOnlyList<CreateCategoryDto> dtos
        );
        Task<CategoryDto> UpdateCategoryAsync(UpdateCategoryDto dto);
        Task<bool> DeleteCategoryAsync(long id);
    }
}
