using Core.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces.Reposiories;
using Core.Specifications;
using Core.Specifications.Products;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Core.Interfaces.Services;
using API.Services;
using Core.Dtos.CreateDto;
using Stripe;

namespace API.Controllers
{
    public class ProductsController : ApiControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IMapper mapper, IProductService productService)
        {
            _mapper = mapper;
            _productService = productService;
        }

        //[Cached(600)]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductDto>>> GetProducts([FromQuery] ProductSpecParams productSpecParams)
        {
            var products = await _productService.GetAllProductsAsync(productSpecParams);
            var produtDtos = _mapper.Map<IReadOnlyList<ProductDto>>(products);

            var count = await _productService.CountAllProductsAsync(productSpecParams);
            return Ok(new Pagination<ProductDto>(productSpecParams.PageNumber, productSpecParams.PageSize, count, produtDtos));
        }

        //[Cached(600)]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Core.Entities.Product>>> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product != null)
            {
                return Ok(_mapper.Map<ProductDto>(product));
            }
            else
            {
                return NotFound(new ApiException(404));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> PostProduct(CreateProductDto productDto)
        {
            var result = await _productService.AddProductAsync(productDto);

            if (result == null)
            {
                return BadRequest(new ApiResponse(400, "Creating fail"));
            }
            return Ok(_mapper.Map<ProductDto>(result));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, CreateProductDto productDto)
        {
            var result = await _productService.UpdateProductAsync(id, productDto);

            if (result == null)
            {
                return BadRequest(new ApiResponse(400, "Updating fail"));
            }
            return Ok(_mapper.Map<ProductDto>(result));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound(new ApiException(404));
            }

            var result = await _productService.DeleteProductAsync(id);

            if (result < 0)
            {
                return BadRequest(new ApiResponse(400, "Deleting fail"));
            }
            return Ok("Delete succesfully");
        }


    }
}
