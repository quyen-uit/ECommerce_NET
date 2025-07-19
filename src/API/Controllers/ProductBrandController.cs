using API.Commons;
using Core.Common;
using Core.Constants;
using Core.Dtos;
using Core.Dtos.ProductBrands;
using Core.Interfaces.Services;
using Core.Specifications.ProductBrands;
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
        public async Task<ActionResult<ApiSuccessResponse<ProductBrandDto>>> GetProductBrand(long id)
        {
            var brandDto = await _brandService.GetProductBrandByIdAsync(id);
            return Ok(ResponseFactory.Ok(brandDto));
        }

        // 🔄 POST: api/productbrand/get-all
        [HttpPost("get-all")]
        public async Task<ActionResult<ApiSuccessResponse<Pagination<ProductBrandDto>>>> GetProductBrands(
            [FromBody] ProductBrandSpecParams specParams
        )
        {
            var result = await _brandService.GetAllProductBrandsAsync(specParams);

            return Ok(ResponseFactory.Ok(result));
        }

        // ✅ POST: api/productbrand/create
        [HttpPost("create")]
        public async Task<ActionResult<ApiSuccessResponse<ProductBrandDto>>> CreateProductBrand(
            [FromBody] CreateProductBrandDto brandDto
        )
        {
            var result = await _brandService.AddOrUpdateProductBrandAsync(brandDto);
            return Ok(ResponseFactory.Ok(result));
        }

        // ✅ POST: api/productbrand/create-many
        [HttpPost("create-many")]
        public async Task<ActionResult<ApiSuccessResponse<IReadOnlyList<ProductBrandDto>>>> CreateProductBrands(
            [FromBody] IReadOnlyList<CreateProductBrandDto> brandDtos
        )
        {
            var result = await _brandService.AddRangeProductBrandAsync(brandDtos);
            return Ok(ResponseFactory.Ok(result));
        }

        // ✅ POST: api/productbrand/update
        [HttpPost("update")]
        public async Task<ActionResult<ApiSuccessResponse<ProductBrandDto>>> UpdateProductBrand(
            [FromBody] CreateProductBrandDto brandDto
        )
        {
            var result = await _brandService.AddOrUpdateProductBrandAsync(brandDto);
            return Ok(ResponseFactory.Ok(result));
        }

        // ✅ DELETE: api/productbrand/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiSuccessResponse<object>>> DeleteProductBrand(long id)
        {
            await _brandService.DeleteProductBrandAsync(id);
            return Ok(ResponseFactory.Ok());
        }

        [HttpDelete("delete-many")]
        public async Task<ActionResult<ApiSuccessResponse<object>>> DeleteBrands([FromBody] List<long> ids)
        {
            await _brandService.DeleteBrandsAsync(ids);
            return Ok(ResponseFactory.Ok());
        }
    }
}
