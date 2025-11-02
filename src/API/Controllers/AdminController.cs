using API.Helpers;
using Core.Interfaces.Services;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController : ApiControllerBase
    {
        private readonly ISoftDeleteAdminService _softDeleteAdminService;
        private readonly ApplicationDbContext _context;

        public AdminController(ISoftDeleteAdminService softDeleteAdminService, ApplicationDbContext context)
        {
            _softDeleteAdminService = softDeleteAdminService;
            _context = context;
        }

        /// <summary>
        /// Get all soft-deleted products
        /// </summary>
        [HttpGet("soft-deleted/products")]
        [Authorize(Policy = "Permission:Product.Manage")]
        public async Task<IActionResult> GetSoftDeletedProducts()
        {
            var result = await _softDeleteAdminService.GetSoftDeletedProductsAsync();
            return Ok(ResponseFactory.Success(result));
        }

        /// <summary>
        /// Get all soft-deleted categories
        /// </summary>
        [HttpGet("soft-deleted/categories")]
        [Authorize(Policy = "Permission:Category.Manage")]
        public async Task<IActionResult> GetSoftDeletedCategories()
        {
            var result = await _softDeleteAdminService.GetSoftDeletedCategoriesAsync();
            return Ok(ResponseFactory.Success(result));
        }

        /// <summary>
        /// Get all soft-deleted brands
        /// </summary>
        [HttpGet("soft-deleted/brands")]
        [Authorize(Policy = "Permission:ProductBrand.Manage")]
        public async Task<IActionResult> GetSoftDeletedBrands()
        {
            var result = await _softDeleteAdminService.GetSoftDeletedBrandsAsync();
            return Ok(ResponseFactory.Success(result));
        }

        /// <summary>
        /// Restore a soft-deleted product
        /// </summary>
        [HttpPost("soft-deleted/products/{id}/restore")]
        [Authorize(Policy = "Permission:Product.Manage")]
        public async Task<IActionResult> RestoreProduct(Guid id)
        {
            var result = await _softDeleteAdminService.RestoreProductAsync(id);
            return Ok(ResponseFactory.Success(result, "Product restored successfully"));
        }

        /// <summary>
        /// Restore a soft-deleted category
        /// </summary>
        [HttpPost("soft-deleted/categories/{id}/restore")]
        [Authorize(Policy = "Permission:Category.Manage")]
        public async Task<IActionResult> RestoreCategory(Guid id)
        {
            var result = await _softDeleteAdminService.RestoreCategoryAsync(id);
            return Ok(ResponseFactory.Success(result, "Category restored successfully"));
        }

        /// <summary>
        /// Restore a soft-deleted brand
        /// </summary>
        [HttpPost("soft-deleted/brands/{id}/restore")]
        [Authorize(Policy = "Permission:ProductBrand.Manage")]
        public async Task<IActionResult> RestoreBrand(Guid id)
        {
            var result = await _softDeleteAdminService.RestoreBrandAsync(id);
            return Ok(ResponseFactory.Success(result, "Brand restored successfully"));
        }

        /// <summary>
        /// Permanently delete a soft-deleted product
        /// </summary>
        [HttpDelete("soft-deleted/products/{id}/permanent")]
        [Authorize(Policy = "Permission:Product.Manage")]
        public async Task<IActionResult> PermanentlyDeleteProduct(Guid id)
        {
            var result = await _softDeleteAdminService.PermanentlyDeleteProductAsync(id);
            return Ok(ResponseFactory.Success(result, "Product permanently deleted"));
        }

        /// <summary>
        /// Permanently delete a soft-deleted category
        /// </summary>
        [HttpDelete("soft-deleted/categories/{id}/permanent")]
        [Authorize(Policy = "Permission:Category.Manage")]
        public async Task<IActionResult> PermanentlyDeleteCategory(Guid id)
        {
            var result = await _softDeleteAdminService.PermanentlyDeleteCategoryAsync(id);
            return Ok(ResponseFactory.Success(result, "Category permanently deleted"));
        }

        /// <summary>
        /// Permanently delete a soft-deleted brand
        /// </summary>
        [HttpDelete("soft-deleted/brands/{id}/permanent")]
        [Authorize(Policy = "Permission:ProductBrand.Manage")]
        public async Task<IActionResult> PermanentlyDeleteBrand(Guid id)
        {
            var result = await _softDeleteAdminService.PermanentlyDeleteBrandAsync(id);
            return Ok(ResponseFactory.Success(result, "Brand permanently deleted"));
        }

        /// <summary>
        /// Get recent audit logs (paginated)
        /// </summary>
        [HttpGet("audit-logs")]
        [Authorize(Policy = "Permission:Permission.Manage")] // High-privilege requirement
        public async Task<IActionResult> GetAuditLogs(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 50,
            [FromQuery] string? userId = null,
            [FromQuery] string? action = null)
        {
            var query = _context.AuditLogs.AsQueryable();

            if (!string.IsNullOrEmpty(userId))
                query = query.Where(a => a.UserId == userId);

            if (!string.IsNullOrEmpty(action))
                query = query.Where(a => a.Action == action);

            var totalCount = await query.CountAsync();
            var logs = await query
                .OrderByDescending(a => a.Timestamp)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Data = logs
            };

            return Ok(ResponseFactory.Success(result));
        }
    }
}
