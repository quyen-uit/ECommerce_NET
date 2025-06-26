using API.Errors;
using API.Helpers;
using Core.Common;
using Core.Constants;
using Core.Dtos;
using Core.Dtos.ProductBrands;
using Core.Interfaces.Services;
using Core.Specifications.ProductBrands;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductBrandController : ApiControllerBase
    {
        private readonly IProductBrandService _brandService;

        public ProductBrandController(IProductBrandService brandService)
        {
            _brandService = brandService;
        }

        // ✅ GET: api/productbrand/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductBrandDto>> GetProductBrand(long id)
        {
            var brandDto = await _brandService.GetProductBrandByIdAsync(id);
            return Ok(brandDto);
        }

        // 🔄 POST: api/productbrand/get-all
        [HttpPost("get-all")]
        public async Task<ActionResult<Pagination<ProductBrandDto>>> GetProductBrands(
            [FromBody] ProductBrandSpecParams specParams
        )
        {
            var result = await _brandService.GetAllProductBrandsAsync(specParams);

            return Ok(result);
        }

        // ✅ POST: api/productbrand/create
        [HttpPost("create")]
        public async Task<ActionResult<ProductBrandDto>> CreateProductBrand(
            [FromBody] CreateProductBrandDto brandDto
        )
        {
            var result = await _brandService.AddOrUpdateProductBrandAsync(brandDto);
            if (result == null)
                return BadRequest(new ApiResponse(400, CommonMessage.CreateFail));

            return Ok(result);
        }

        // ✅ POST: api/productbrand/create-many
        [HttpPost("create-many")]
        public async Task<ActionResult<IReadOnlyList<ProductBrandDto>>> CreateProductBrands(
            [FromBody] IReadOnlyList<CreateProductBrandDto> brandDtos
        )
        {
            var result = await _brandService.AddRangeProductBrandAsync(brandDtos);
            if (result == null || !result.Any())
                return BadRequest(new ApiResponse(400, CommonMessage.CreateFail));

            return Ok(result);
        }

        // ✅ POST: api/productbrand/update
        [HttpPost("update")]
        public async Task<ActionResult<ProductBrandDto>> UpdateProductBrand(
            [FromBody] CreateProductBrandDto brandDto
        )
        {
            var result = await _brandService.AddOrUpdateProductBrandAsync(brandDto);
            return Ok(result);
        }

        // ✅ DELETE: api/productbrand/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteProductBrand(long id)
        {
            await _brandService.DeleteProductBrandAsync(id);
            return Ok(CommonMessage.DeleteSuccess);
        }
    }
}
