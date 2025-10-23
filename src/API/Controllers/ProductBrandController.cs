using API.Commons.Response;
using API.Helpers;
using Core.Common;
using Core.Dtos.ProductBrands;
using Core.Interfaces.Services;
using Core.Specifications.ProductBrands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/brand")]
    public class ProductBrandController : ApiControllerBase
    {
        private readonly IProductBrandService _brandService;

        public ProductBrandController(IProductBrandService brandService)
        {
            _brandService = brandService;
        }

        // ✅ GET: api/productbrand/{id}
        [HttpGet("{id}")]
        [Authorize(Policy = "Permission:Brand.Read")]
        public async Task<ActionResult<ApiSuccessResponse<ProductBrandDto>>> GetProductBrand(Guid id)
        {
            var brandDto = await _brandService.GetProductBrandByIdAsync(id);
            return Ok(ResponseFactory.Ok(brandDto));
        }

        // 🔄 POST: api/productbrand/get-all
        [HttpPost("search")]
        [Authorize(Policy = "Permission:Brand.Read")]
        public async Task<ActionResult<ApiSuccessResponse<Pagination<ProductBrandDto>>>> GetProductBrands(
                    [FromBody] ProductBrandSpecParams specParams
                )
        {
            var result = await _brandService.GetAllProductBrandsAsync(specParams);

            return Ok(ResponseFactory.Ok(result));
        }

        // ✅ POST: api/productbrand/create
        [HttpPost("create")]
        [Authorize(Policy = "Permission:Brand.Create")]
        public async Task<ActionResult<ApiSuccessResponse<ProductBrandDto>>> CreateProductBrand(
            [FromBody] CreateProductBrandDto brandDto
        )
        {
            var result = await _brandService.AddOrUpdateProductBrandAsync(brandDto);
            return Ok(ResponseFactory.Ok(result));
        }

        // ✅ POST: api/productbrand/create-many
        [HttpPost("create-many")]
        [Authorize(Policy = "Permission:Brand.Create")]
        public async Task<ActionResult<ApiSuccessResponse<IReadOnlyList<ProductBrandDto>>>> CreateProductBrands(
            [FromBody] IReadOnlyList<CreateProductBrandDto> brandDtos
        )
        {
            var result = await _brandService.AddRangeProductBrandAsync(brandDtos);
            return Ok(ResponseFactory.Ok(result));
        }

        // ✅ POST: api/productbrand/update
        [HttpPost("update")]
        [Authorize(Policy = "Permission:Brand.Update")]
        public async Task<ActionResult<ApiSuccessResponse<ProductBrandDto>>> UpdateProductBrand(
            [FromBody] CreateProductBrandDto brandDto
        )
        {
            var result = await _brandService.AddOrUpdateProductBrandAsync(brandDto);
            return Ok(ResponseFactory.Ok(result));
        }

        // ✅ DELETE: api/productbrand/{id}
        [HttpDelete("{id}")]
        [Authorize(Policy = "Permission:Brand.Delete")]
        public async Task<ActionResult<ApiSuccessResponse<object>>> DeleteProductBrand(Guid id)
        {
            await _brandService.DeleteProductBrandAsync(id);
            return Ok(ResponseFactory.Ok());
        }

        [HttpDelete("delete-many")]
        [Authorize(Policy = "Permission:Brand.Delete")]
        public async Task<ActionResult<ApiSuccessResponse<object>>> DeleteBrands([FromBody] List<Guid> ids)
        {
            await _brandService.DeleteBrandsAsync(ids);
            return Ok(ResponseFactory.Ok());
        }
    }
}
