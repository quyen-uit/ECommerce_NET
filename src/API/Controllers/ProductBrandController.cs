using Core.Dtos;
using Core.Dtos.CreateDto;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/brand")]
    public class ProductBrandController : ApiControllerBase
    {
        private readonly IProductBrandService _productBrandService;
        private readonly IMapper _mapper;

        public ProductBrandController(IProductBrandService productBrandService, IMapper mapper)
        {
            _productBrandService = productBrandService;
            _mapper = mapper;
        }

        // GET: api/ProductBrand
        [HttpGet("all")]
        public async Task<ActionResult<IReadOnlyList<ProductBrandDto>>> GetProductBrands()
        {
            var productBrands = await _productBrandService.GetAllProductBrandsAsync();
            var productBrandDtos = _mapper.Map<IReadOnlyList<ProductBrandDto>>(productBrands);

            return Ok(productBrandDtos);
        }


        // POST: api/ProductBrand
        [HttpPost]
        public async Task<ActionResult<ProductBrandDto>> PostProductBrand(CreateProductBrandDto productBrandDto)
        {
            var result = await _productBrandService.AddProductBrandAsync(productBrandDto);

            if (result == null)
            {
                return BadRequest(new ApiResponse(400, "Creating fail"));
            }
            return Ok(_mapper.Map<ProductBrandDto>(result));
        }

        // POST: api/ProductBrand/many
        [HttpPost("many")]
        public async Task<ActionResult<ProductBrandDto>> PostProductBrands(IReadOnlyList<CreateProductBrandDto> productBrandDtos)
        {
            var result = await _productBrandService.AddRangeProductBrandAsync(productBrandDtos);

            if (result == null)
            {
                return BadRequest(new ApiResponse(400, "Creating fail"));
            }
            return Ok(_mapper.Map<IReadOnlyList<ProductBrandDto>>(result));
        }

        // PUT: api/ProductBrand/5
        [HttpPut]
        public async Task<IActionResult> PutProductBrand(ProductBrandDto productBrandDto)
        {
            var result = await _productBrandService.UpdateProductBrandAsync(productBrandDto);

            if (result == null)
            {
                return BadRequest(new ApiResponse(400, "Updating fail"));
            }
            return Ok(_mapper.Map<ProductBrandDto>(result));
        }

        // DELETE: api/ProductBrand/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductBrand(int id)
        {
            var productBrand = await _productBrandService.GetProductBrandByIdAsync(id);
            if (productBrand == null)
            {
                return NotFound(new ApiException(404));
            }

            var result = await _productBrandService.DeleteProductBrandAsync(id);

            if (result < 0)
            {
                return BadRequest(new ApiResponse(400, "Deleting fail"));
            }
            return Ok("Delete succesfully");
        }
    }
}
