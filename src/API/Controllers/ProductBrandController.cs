using API.Errors;
using API.Helpers;
using Core.Dtos;
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
            if (brandDto == null)
                return NotFound(new ApiException(404, "Brand not found"));

            return Ok(brandDto);
        }

        // 🔄 POST: api/productbrand/get-all
        [HttpPost("get-all")]
        public async Task<ActionResult<Pagination<ProductBrandDto>>> GetProductBrands(
            [FromBody] ProductBrandSpecParams specParams
        )
        {
            var brandDtos = await _brandService.GetAllProductBrandsAsync(specParams);

            return Ok(
                new Pagination<ProductBrandDto>(
                    pageNumber: specParams.PageNumber,
                    pageSize: specParams.PageSize,
                    pageCount: await _brandService.CountAllAsync(specParams),
                    data: brandDtos
                )
            );
        }

        // ✅ POST: api/productbrand/create
        [HttpPost("create")]
        public async Task<ActionResult<ProductBrandDto>> CreateProductBrand(
            [FromBody] CreateProductBrandDto brandDto
        )
        {
            var result = await _brandService.AddProductBrandAsync(brandDto);
            if (result == null)
                return BadRequest(new ApiResponse(400, "Failed to create brand"));

            return CreatedAtAction(nameof(GetProductBrand), new { id = result.Id }, result);
        }

        // ✅ POST: api/productbrand/create-many
        [HttpPost("create-many")]
        public async Task<ActionResult<IReadOnlyList<ProductBrandDto>>> CreateProductBrands(
            [FromBody] IReadOnlyList<CreateProductBrandDto> brandDtos
        )
        {
            var result = await _brandService.AddRangeProductBrandAsync(brandDtos);
            if (result == null || !result.Any())
                return BadRequest(new ApiResponse(400, "Failed to create brands"));

            return Ok(result);
        }

        // ✅ POST: api/productbrand/update
        [HttpPost("update")]
        public async Task<ActionResult<ProductBrandDto>> UpdateProductBrand(
            [FromBody] UpdateProductBrandDto brandDto
        )
        {
            var result = await _brandService.UpdateProductBrandAsync(brandDto);
            if (result == null)
                return NotFound(new ApiException(404, "Brand not found"));

            return Ok(result);
        }

        // ✅ DELETE: api/productbrand/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteProductBrand(long id)
        {
            var success = await _brandService.DeleteProductBrandAsync(id);
            if (!success)
                return NotFound(new ApiException(404, "Brand not found"));

            return Ok(new ApiResponse(200, "Brand deleted successfully"));
        }
    }
}
