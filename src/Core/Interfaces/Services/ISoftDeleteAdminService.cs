using Core.Dtos.Admin;

namespace Core.Interfaces.Services
{
    public interface ISoftDeleteAdminService
    {
        Task<List<SoftDeletedItemDto>> GetSoftDeletedProductsAsync();
        Task<List<SoftDeletedItemDto>> GetSoftDeletedCategoriesAsync();
        Task<List<SoftDeletedItemDto>> GetSoftDeletedBrandsAsync();
        Task<bool> RestoreProductAsync(Guid id);
        Task<bool> RestoreCategoryAsync(Guid id);
        Task<bool> RestoreBrandAsync(Guid id);
        Task<bool> PermanentlyDeleteProductAsync(Guid id);
        Task<bool> PermanentlyDeleteCategoryAsync(Guid id);
        Task<bool> PermanentlyDeleteBrandAsync(Guid id);
    }
}
