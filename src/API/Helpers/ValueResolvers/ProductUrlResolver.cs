using API.Dtos;
using AutoMapper;
using Core.Entities;

namespace API.Helpers.ValueResolvers
{
    public class ProductUrlResolver : IValueResolver<Product, ProductDto, string>
    {
        private readonly IConfiguration _config;

        public ProductUrlResolver(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(Product source, ProductDto destination, string destMember, ResolutionContext context)
        {
            destination.PhotoUrl = _config["ApiUrl"] + source.PhotoUrl;
            return destination.PhotoUrl;
        }
    }
}
