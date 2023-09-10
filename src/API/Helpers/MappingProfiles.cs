using API.Dtos;
using API.Helpers.ValueResolvers;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;

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
            CreateMap<ProductType, ProductTypeDto>();
            CreateMap<ProductBrand, ProductBrandDto>();

            CreateMap<Address, AddressDto>();
            CreateMap<AddressDto, Address>();

            CreateMap<CustomerBasket, CustomerBasketDto>();
            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();
            CreateMap<BasketItem, BasketItemDto>();
        }
    }
}
