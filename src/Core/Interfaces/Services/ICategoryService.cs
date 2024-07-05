using Core.Dtos;
using Core.Dtos.CreateDto;
using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<IReadOnlyList<Category>> GetAllCategoriesAsync(bool? isActive);
        Task<Category> GetCategoryByIdAsync(int id);
        Task<Category> AddCategoryAsync(CreateCategoryDto category);
        Task<IReadOnlyList<Category>> AddRangeCategoryAsync(IReadOnlyList<CreateCategoryDto> categories);
        Task<Category> UpdateCategoryAsync(CategoryDto category);
        Task<int> DeleteCategoryAsync(int id);
    }

}
