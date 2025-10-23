using API.Commons.Response;
using API.Helpers;
using Core.Common;
using Core.Dtos.ProductSkus;
using Core.Interfaces.Services;
using Core.Specifications.ProductSkus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductSkuController : ApiControllerBase
    {
        private readonly IProductSkuService _productSkuService;

        public ProductSkuController(IProductSkuService productSkuService)
        {
            _productSkuService = productSkuService;
        }

        [HttpPost("search")]
        [Authorize(Policy = "Permission:ProductSku.Read")]
        public async Task<ActionResult<ApiSuccessResponse<Pagination<ProductSkuDto>>>> GetProductSkusFilterByName([FromBody] ProductSkuSpecParams productSkuSpecParams)
        {
            var result = await _productSkuService.GetAllProductSkusAsync(productSkuSpecParams);
            return Ok(ResponseFactory.Ok(result));
        }

        //[Cached(600)]
        [HttpGet("{id}")]
        [Authorize(Policy = "Permission:ProductSku.Read")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiSuccessResponse<ProductSkuDto>>> GetProductSku(Guid id)
        {
            var productSku = await _productSkuService.GetProductSkuByIdAsync(id);
            return Ok(ResponseFactory.Ok(productSku));
        }

        [HttpPost("create")]
        [Authorize(Policy = "Permission:ProductSku.Create")]
        public async Task<ActionResult<ApiSuccessResponse<ProductSkuDto>>> PostProductSku([FromBody] CreateProductSkuDto productSkuDto)
        {
            var result = await _productSkuService.AddOrUpdateProductSkuAsync(productSkuDto);
            return Ok(ResponseFactory.Ok(result));
        }

        [HttpPost("update")]
        [Authorize(Policy = "Permission:ProductSku.Update")]
        public async Task<ActionResult<ApiSuccessResponse<ProductSkuDto>>> PutProductSku([FromBody] CreateProductSkuDto productSkuDto)
        {
            var result = await _productSkuService.AddOrUpdateProductSkuAsync(productSkuDto);
            return Ok(ResponseFactory.Ok(result));
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Permission:ProductSku.Delete")]
        public async Task<ActionResult<ApiSuccessResponse<object>>> DeleteProductSku(Guid id)
        {
            await _productSkuService.DeleteProductSkuAsync(id);
            return Ok(ResponseFactory.Ok());
        }
    }
}
