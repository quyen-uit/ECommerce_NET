﻿using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces.Reposiories;
using Core.Specifications;
using Core.Specifications.Parameters;
using Core.Specifications.Products;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    public class ProductsController : ApiControllerBase
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<ProductBrand> _brandRepository;
        private readonly IGenericRepository<ProductType> _typeRepository;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productRepository, IGenericRepository<ProductBrand> brandRepository, IGenericRepository<ProductType> typeRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _brandRepository = brandRepository;
            _typeRepository = typeRepository;
            _mapper = mapper;
        }

        //[Cached(600)]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductDto>>> GetProducts([FromQuery] ProductSpecParams productSpecParams)
        {
            var spec = new ProductWithTypesAndBrandsSpecification(productSpecParams);
            var countSpec = new ProductsWithFiltersForCountSpecification(productSpecParams);

            var count = await _productRepository.CountAsync(countSpec);

            var products = await _productRepository.GetAllWithSpecAsync(spec);
            var produtDtos = _mapper.Map<IReadOnlyList<ProductDto>>(products);

            return Ok(new Pagination<ProductDto>(productSpecParams.PageNumber, productSpecParams.PageSize, count, produtDtos));
        }

        //[Cached(600)]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Product>>> GetProduct(int id)
        {
            var spec = new ProductWithTypesAndBrandsSpecification(id);

            var product = await _productRepository.GetEntityWithSpecAsync(spec);
            if (product != null)
            {
                return Ok(_mapper.Map<ProductDto>(product));
            }
            else
            {
                return NotFound(new ApiException(404));
            }
        }

        //[Cached(600)]
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductTypeDto>>> GetProductTypes()
        {
            var types = await _typeRepository.GetAllAsync();
            return Ok(_mapper.Map<IReadOnlyList<ProductTypeDto>>(types));
        }
        //[Cached(600)]
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrandDto>>> GetProductBrands()
        {
            var types = await _brandRepository.GetAllAsync();
            return Ok(_mapper.Map<IReadOnlyList<ProductBrandDto>>(types));
        }
    }
}
