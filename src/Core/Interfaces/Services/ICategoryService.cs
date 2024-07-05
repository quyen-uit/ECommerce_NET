using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<IReadOnlyList<Category>> GetAllCategoriesAsync(bool? isActive);
        Task<Category> GetCategoryByIdAsync(int id);
        Task<int> AddCategoryAsync(Category category);
        Task<int> AddRangeCategoryAsync(IReadOnlyList<Category> categories);
        Task<int> UpdateCategoryAsync(Category category);
        Task<int> DeleteCategoryAsync(int id);
    }

}
