using Core.Dtos.Admin;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class SoftDeleteAdminService : ISoftDeleteAdminService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SoftDeleteAdminService> _logger;

        public SoftDeleteAdminService(ApplicationDbContext context, ILogger<SoftDeleteAdminService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<SoftDeletedItemDto>> GetSoftDeletedProductsAsync()
        {
            var products = await _context.Products
                .IgnoreQueryFilters()
                .Where(p => p.IsDeleted)
                .OrderByDescending(p => p.UpdatedAt)
                .Select(p => new SoftDeletedItemDto
                {
                    Id = p.Id,
                    EntityType = "Product",
                    DisplayName = p.Name,
                    DeletedAt = p.UpdatedAt
                })
                .ToListAsync();

            return products;
        }

        public async Task<List<SoftDeletedItemDto>> GetSoftDeletedCategoriesAsync()
        {
            var categories = await _context.Categories
                .IgnoreQueryFilters()
                .Where(c => c.IsDeleted)
                .OrderByDescending(c => c.UpdatedAt)
                .Select(c => new SoftDeletedItemDto
                {
                    Id = c.Id,
                    EntityType = "Category",
                    DisplayName = c.Name,
                    DeletedAt = c.UpdatedAt
                })
                .ToListAsync();

            return categories;
        }

        public async Task<List<SoftDeletedItemDto>> GetSoftDeletedBrandsAsync()
        {
            var brands = await _context.ProductBrands
                .IgnoreQueryFilters()
                .Where(b => b.IsDeleted)
                .OrderByDescending(b => b.UpdatedAt)
                .Select(b => new SoftDeletedItemDto
                {
                    Id = b.Id,
                    EntityType = "ProductBrand",
                    DisplayName = b.Name,
                    DeletedAt = b.UpdatedAt
                })
                .ToListAsync();

            return brands;
        }

        public async Task<bool> RestoreProductAsync(Guid id)
        {
            var product = await _context.Products
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted);

            if (product == null)
                throw new NotFoundException("Soft-deleted product not found");

            product.IsDeleted = false;
            product.UpdatedAt = DateTime.UtcNow;
            _context.Update(product);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Product {ProductId} ({ProductName}) restored from soft delete", id, product.Name);
            return true;
        }

        public async Task<bool> RestoreCategoryAsync(Guid id)
        {
            var category = await _context.Categories
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(c => c.Id == id && c.IsDeleted);

            if (category == null)
                throw new NotFoundException("Soft-deleted category not found");

            category.IsDeleted = false;
            category.UpdatedAt = DateTime.UtcNow;
            _context.Update(category);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Category {CategoryId} ({CategoryName}) restored from soft delete", id, category.Name);
            return true;
        }

        public async Task<bool> RestoreBrandAsync(Guid id)
        {
            var brand = await _context.ProductBrands
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(b => b.Id == id && b.IsDeleted);

            if (brand == null)
                throw new NotFoundException("Soft-deleted brand not found");

            brand.IsDeleted = false;
            brand.UpdatedAt = DateTime.UtcNow;
            _context.Update(brand);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Brand {BrandId} ({BrandName}) restored from soft delete", id, brand.Name);
            return true;
        }

        public async Task<bool> PermanentlyDeleteProductAsync(Guid id)
        {
            var product = await _context.Products
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted);

            if (product == null)
                throw new NotFoundException("Soft-deleted product not found");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            _logger.LogWarning("Product {ProductId} ({ProductName}) permanently deleted", id, product.Name);
            return true;
        }

        public async Task<bool> PermanentlyDeleteCategoryAsync(Guid id)
        {
            var category = await _context.Categories
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(c => c.Id == id && c.IsDeleted);

            if (category == null)
                throw new NotFoundException("Soft-deleted category not found");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            _logger.LogWarning("Category {CategoryId} ({CategoryName}) permanently deleted", id, category.Name);
            return true;
        }

        public async Task<bool> PermanentlyDeleteBrandAsync(Guid id)
        {
            var brand = await _context.ProductBrands
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(b => b.Id == id && b.IsDeleted);

            if (brand == null)
                throw new NotFoundException("Soft-deleted brand not found");

            _context.ProductBrands.Remove(brand);
            await _context.SaveChangesAsync();

            _logger.LogWarning("Brand {BrandId} ({BrandName}) permanently deleted", id, brand.Name);
            return true;
        }
    }
}
