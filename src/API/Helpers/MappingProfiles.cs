using API.Dtos;
using API.Helpers.ValueResolvers;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(d => d.ProductType, opt => opt.MapFrom(s => s.ProductType.Name))
                .ForMember(d => d.ProductBrand, opt => opt.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.PhotoUrl, opt => opt.MapFrom<ProductUrlResolver>());

        }
    }
}
