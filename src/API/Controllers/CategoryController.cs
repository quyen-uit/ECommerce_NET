using API.Commons.Response;
using API.Helpers;
using Core.Common;
using Core.Dtos.Categories;
using Core.Interfaces.Services;
using Core.Specifications.Categories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CategoryController : ApiControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiSuccessResponse<CategoryDto>>> GetCategory(long id)
        {
            var dto = await _categoryService.GetCategoryByIdAsync(id);
            return Ok(ResponseFactory.Ok(dto));
        }

        [HttpPost("get-all")]
        public async Task<ActionResult<ApiSuccessResponse<Pagination<CategoryDto>>>> GetCategories(
            [FromBody] CategorySpecParams specParams
        )
        {
            var result = await _categoryService.GetAllCategoriesAsync(specParams);
            return Ok(ResponseFactory.Ok(result));
        }

        [HttpGet("hierarchy")]
        public async Task<ActionResult<ApiSuccessResponse<List<CategoryNodeDto>>>> GetHierarchyCategories()
        {
            var result = await _categoryService.GetCategoriesHierarchyAsync();
            return Ok(ResponseFactory.Ok(result));
        }

        [HttpPost("create")]
        public async Task<ActionResult<ApiSuccessResponse<CategoryDto>>> CreateCategory(
            [FromBody] CreateCategoryDto dto
        )
        {
            var result = await _categoryService.AddOrUpdateCategoryAsync(dto);
            return Ok(ResponseFactory.Ok(result));
        }

        [HttpPost("create-many")]
        public async Task<ActionResult<ApiSuccessResponse<IReadOnlyList<CategoryDto>>>> CreateCategories(
            [FromBody] IReadOnlyList<CreateCategoryDto> dtos
        )
        {
            var result = await _categoryService.AddRangeCategoryAsync(dtos);
            return Ok(ResponseFactory.Ok(result));
        }

        [HttpPost("update")]
        public async Task<ActionResult<ApiSuccessResponse<CategoryDto>>> UpdateCategory(
            [FromBody] CreateCategoryDto dto
        )
        {
            var result = await _categoryService.AddOrUpdateCategoryAsync(dto);
            return Ok(ResponseFactory.Ok(result));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiSuccessResponse<object>>> DeleteCategory(long id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return Ok(ResponseFactory.Ok());
        }
    }
}
