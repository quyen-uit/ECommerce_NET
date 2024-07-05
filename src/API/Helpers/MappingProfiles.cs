using Core.Dtos;
using API.Helpers.ValueResolvers;
using AutoMapper;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Dtos.CreateDto;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(d => d.ProductType, opt => opt.MapFrom(s => s.Category.Name))
                .ForMember(d => d.ProductBrand, opt => opt.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.PhotoUrl, opt => opt.MapFrom<ProductUrlResolver>());
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<ProductBrand, ProductBrandDto>().ReverseMap();
            CreateMap<CreateProductBrandDto, ProductBrand>();
            CreateMap<Color, ColorDto>().ReverseMap();
            CreateMap<CreateColorDto, Color>();

            CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();

            CreateMap<CustomerBasket, CustomerBasketDto>().ReverseMap();
            CreateMap<BasketItemDto, BasketItem>().ReverseMap();

            CreateMap<AddressDto, Core.Entities.OrderAggregate.Address>();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Item.ProductId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Item.ProductName))
                .ForMember(d => d.PhotoUrl, o => o.MapFrom(s => s.Item.PhotoUrl))
                .ForMember(d => d.PhotoUrl, o => o.MapFrom<OrderItemUrlResolver>());

        }
    }
}
