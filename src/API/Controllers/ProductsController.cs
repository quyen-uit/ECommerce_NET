using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Core.Specifications.Parameters;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    public class ProductsController : ApiControllerBase
    {
        private readonly IGenericRepository<Product> _repository;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductDto>>> GetProducts([FromQuery]ProductSpecParams productSpecParams)
        {
            var spec = new ProductWithTypesAndBrandsSpecification(productSpecParams);
            var countSpec = new ProductsWithFiltersForCountSpecification(productSpecParams);

            var count = await _repository.CountAsync(countSpec);

            var products = await _repository.GetAllWithSpec(spec);
            var produtDtos = _mapper.Map<IReadOnlyList<ProductDto>>(products);

            return Ok(new Pagination<ProductDto>(productSpecParams.PageIndex, productSpecParams.PageSize,  count, produtDtos));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Product>>> GetProduct(int id)
        {
            var spec = new ProductWithTypesAndBrandsSpecification(id);

            var product = await _repository.GetEntityWithSpec(spec);

            return Ok(_mapper.Map<ProductDto>(product));
        }

    }
}
