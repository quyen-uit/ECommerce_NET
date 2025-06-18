using API.Errors;
using Core.Common;
using Core.Constants;
using Core.Dtos;
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
        public async Task<ActionResult<Pagination<ProductDto>>> GetProductsFilterByName([FromBody] ProductFilterByNameSpecParams productSpecParams)
        {
            var result = await _productService.GetAllProductFilterByNameAsync(productSpecParams);
            return Ok(result);
        }
        #endregion

        //[Cached(600)]
        [HttpPost("get-all-by-id")]
        public async Task<ActionResult<Pagination<ProductDto>>> GetProducts([FromBody] ProductSpecParams productSpecParams)
        {
            var result = await _productService.GetAllProductsAsync(productSpecParams);
            return Ok(result);
        }

        //[Cached(600)]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            return Ok(product);
        }

        [HttpPost("create")]
        public async Task<ActionResult<ProductDto>> PostProduct(CreateProductDto productDto)
        {
            var result = await _productService.AddProductAsync(productDto);
            if (result == null)
                return BadRequest(new ApiResponse(400, CommonMessage.CreateFail));

            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> PutProduct(UpdateProductDto productDto)
        {
            var result = await _productService.UpdateProductAsync(productDto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProductAsync(id);
            return Ok(CommonMessage.DeleteSuccess);
        }


    }
}
