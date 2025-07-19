using API.Commons;
using Core.Common;
using Core.Dtos.Products;
using Core.Interfaces.Services;
using Core.Specifications.Products;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController : ApiControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        #region Admin APIs
        //[Cached(600)]
        [HttpPost("get-all")]
        public async Task<ActionResult<ApiSuccessResponse<Pagination<ProductDto>>>> GetProductsFilterByName([FromBody] ProductFilterByNameSpecParams productSpecParams)
        {
            var result = await _productService.GetAllProductFilterByNameAsync(productSpecParams);
            return Ok(ResponseFactory.Ok(result));
        }
        #endregion

        //[Cached(600)]
        [HttpPost("get-all-by-id")]
        public async Task<ActionResult<ApiSuccessResponse<Pagination<ProductDto>>>> GetProducts([FromBody] ProductSpecParams productSpecParams)
        {
            var result = await _productService.GetAllProductsAsync(productSpecParams);
            return Ok(ResponseFactory.Ok(result));
        }

        //[Cached(600)]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiSuccessResponse<ProductDto>>> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            return Ok(ResponseFactory.Ok(product));
        }

        [HttpPost("create")]
        public async Task<ActionResult<ApiSuccessResponse<ProductDto>>> PostProduct([FromBody] CreateProductDto productDto)
        {
            var result = await _productService.AddOrUpdateProductAsync(productDto);
            return Ok(ResponseFactory.Ok(result));
        }

        [HttpPost("update")]
        public async Task<ActionResult<ApiSuccessResponse<ProductDto>>> PutProduct([FromBody] CreateProductDto productDto)
        {
            var result = await _productService.AddOrUpdateProductAsync(productDto);
            return Ok(ResponseFactory.Ok(result));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiSuccessResponse<object>>> DeleteProduct(int id)
        {
            await _productService.DeleteProductAsync(id);
            return Ok(ResponseFactory.Ok());
        }
    }
}
