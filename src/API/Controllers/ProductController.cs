using API.Commons.Response;
using API.Helpers;
using Core.Common;
using Core.Dtos.Products;
using Core.Interfaces.Services;
using Core.Specifications.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductController : ApiControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        #region Admin APIs
        //[Cached(600)]
        [HttpPost("search")]
        [Authorize(Policy = "Permission:Product.Read")]
        public async Task<ActionResult<ApiSuccessResponse<Pagination<ProductDto>>>> GetProductsFilterByName([FromBody] ProductFilterByNameSpecParams productSpecParams)
        {
            var result = await _productService.GetAllProductFilterByNameAsync(productSpecParams);
            return Ok(ResponseFactory.Ok(result));
        }
        #endregion

        //[Cached(600)]
        [HttpPost("get-all-by-id")]
        [Authorize(Policy = "Permission:Product.Read")]
        public async Task<ActionResult<ApiSuccessResponse<Pagination<ProductDto>>>> GetProducts([FromBody] ProductSpecParams productSpecParams)
        {
            var result = await _productService.GetAllProductsAsync(productSpecParams);
            return Ok(ResponseFactory.Ok(result));
        }

        //[Cached(600)]
        [HttpGet("{id}")]
        [Authorize(Policy = "Permission:Product.Read")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiSuccessResponse<ProductDto>>> GetProduct(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            return Ok(ResponseFactory.Ok(product));
        }

        [HttpPost("create")]
        [Authorize(Policy = "Permission:Product.Create")]
        public async Task<ActionResult<ApiSuccessResponse<ProductDto>>> PostProduct([FromBody] CreateProductDto productDto)
        {
            var result = await _productService.AddOrUpdateProductAsync(productDto);
            return Ok(ResponseFactory.Ok(result));
        }

        [HttpPost("update")]
        [Authorize(Policy = "Permission:Product.Update")]
        public async Task<ActionResult<ApiSuccessResponse<ProductDto>>> PutProduct([FromBody] CreateProductDto productDto)
        {
            var result = await _productService.AddOrUpdateProductAsync(productDto);
            return Ok(ResponseFactory.Ok(result));
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Permission:Product.Delete")]
        public async Task<ActionResult<ApiSuccessResponse<object>>> DeleteProduct(Guid id)
        {
            await _productService.DeleteProductAsync(id);
            return Ok(ResponseFactory.Ok());
        }
    }
}
