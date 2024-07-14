using Core.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Core.Interfaces.Services;
using API.Services;
using Core.Dtos.CreateDto;

namespace API.Controllers
{
    public class ProductColorController : ApiControllerBase
    {
        private readonly IProductService _productColorService;
        private readonly IMapper _mapper;

        public ProductColorController(IMapper mapper, IProductService productColorService)
        {
            _mapper = mapper;
            _productColorService = productColorService;
        }

        //[Cached(600)]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ProductColor>>> GetProductColor(int id)
        {
            var productColor = await _productColorService.GetProductColorsByIdAsync(id);

            if (productColor != null)
            {
                return Ok(_mapper.Map<ProductColorDto>(productColor));
            }
            else
            {
                return NotFound(new ApiException(404));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ProductColorDto>> PostProductColor(CreateProductColorDto productColorDto)
        {
            var result = await _productColorService.AddProductColorAsync(productColorDto);

            if (result == null)
            {
                return BadRequest(new ApiResponse(400, "Creating fail"));
            }
            return Ok(_mapper.Map<ProductColorDto>(result));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductColor(int id, CreateProductColorDto productColorDto)
        {
            var result = await _productColorService.UpdateProductColorAsync(id, productColorDto);

            if (result == null)
            {
                return BadRequest(new ApiResponse(400, "Updating fail"));
            }
            return Ok(_mapper.Map<ProductColorDto>(result));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductColor(int id)
        {
            var productColorBrand = await _productColorService.GetProductColorsByIdAsync(id);
            if (productColorBrand == null)
            {
                return NotFound(new ApiException(404));
            }

            var result = await _productColorService.DeleteProductColorAsync(id);

            if (result < 0)
            {
                return BadRequest(new ApiResponse(400, "Deleting fail"));
            }
            return Ok("Delete succesfully");
        }


    }
}
