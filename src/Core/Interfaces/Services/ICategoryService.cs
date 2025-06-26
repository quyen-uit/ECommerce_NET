using Core.Common;
using Core.Dtos;
using Core.Dtos.Categories;
using Core.Specifications.Categories;

namespace Core.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<Pagination<CategoryDto>> GetAllCategoriesAsync(CategorySpecParams specParams);
        Task<List<CategoryNodeDto>> GetCategoriesHierarchyAsync();
        Task<CategoryDto> GetCategoryByIdAsync(long id);
        Task<CategoryDto> AddOrUpdateCategoryAsync(CreateCategoryDto dto);
        Task<IReadOnlyList<CategoryDto>> AddRangeCategoryAsync(
            IReadOnlyList<CreateCategoryDto> dtos
        );
        Task DeleteCategoryAsync(long id);
    }
}
