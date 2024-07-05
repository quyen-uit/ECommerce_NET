using API.Dtos;
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
        [HttpGet]
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
            var productBrand = _mapper.Map<ProductBrand>(productBrandDto);
            var result = await _productBrandService.AddProductBrandAsync(productBrand);

            if (result < 0)
            {
                return BadRequest(new ApiResponse(400, "Creating fail"));
            }
            return Ok(_mapper.Map<ProductBrandDto>(productBrand));
        }

        // POST: api/ProductBrand/many
        [HttpPost("many")]
        public async Task<ActionResult<ProductBrandDto>> PostProductBrands(IReadOnlyList<CreateProductBrandDto> productBrandDtos)
        {
            var productBrands = _mapper.Map<IReadOnlyList<ProductBrand>>(productBrandDtos);
            var result = await _productBrandService.AddRangeProductBrandAsync(productBrands);

            if (result < 0)
            {
                return BadRequest(new ApiResponse(400, "Creating fail"));
            }
            return Ok(_mapper.Map<IReadOnlyList<ProductBrandDto>>(productBrands));
        }

        // PUT: api/ProductBrand/5
        [HttpPut]
        public async Task<IActionResult> PutProductBrand(ProductBrandDto productBrandDto)
        {
            var productBrand = _mapper.Map<ProductBrand>(productBrandDto);
            var result = await _productBrandService.UpdateProductBrandAsync(productBrand);

            if (result < 0)
            {
                return BadRequest(new ApiResponse(400, "Updating fail"));
            }
            return Ok(_mapper.Map<ProductBrandDto>(productBrand));
        }

        // DELETE: api/ProductBrand/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductBrand(int id)
        {
            var productBrand = await _productBrandService.GetProductBrandByIdAsync(id);
            if (productBrand == null)
            {
                return NotFound("Not found productBrand");
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
