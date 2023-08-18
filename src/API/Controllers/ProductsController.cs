using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController : ApiControllerBase
    {
        private readonly IGenericRepository<Product> _repository;

        public ProductsController(IGenericRepository<Product> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
           var spec = new ProductWithTypesAndBrandsSpecification();
                      
           var products = await _repository.GetAllWithSpec(spec);

           return Ok(products);
        }

        [HttpGet("id")]
        public async Task<ActionResult<List<Product>>> GetProduct(int id)
        {
            var spec = new ProductWithTypesAndBrandsSpecification(id);

            var products = await _repository.GetEntityWithSpec(spec);

            return Ok(products);
        }

    }
}
