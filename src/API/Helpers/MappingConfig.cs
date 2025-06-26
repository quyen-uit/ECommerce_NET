using Core.Dtos;
using Core.Entities;
using Mapster;
using Core.Dtos.Products;
using Core.Dtos.PriceAdjustments;
using Core.Dtos.Images;
using Core.Dtos.ProductSkus;
using Core.Dtos.ProductBrands;
using Core.Dtos.Categories;
using Core.Dtos.Sizes;
using Core.Dtos.Colors;

public static class MapsterConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<CreateColorDto, Color>.NewConfig();
        TypeAdapterConfig<Color, ColorDto>.NewConfig().TwoWays();

        TypeAdapterConfig<CreateSizeDto, Size>.NewConfig();
        TypeAdapterConfig<Size, SizeDto>.NewConfig();

        TypeAdapterConfig<CreateCategoryDto, Category>.NewConfig();
        TypeAdapterConfig<Category, CategoryDto>.NewConfig().TwoWays();

        TypeAdapterConfig<CreateProductBrandDto, ProductBrand>.NewConfig();
        TypeAdapterConfig<ProductBrand, ProductBrandDto>.NewConfig().TwoWays();

        TypeAdapterConfig<CreateProductDto, Product>.NewConfig();
        TypeAdapterConfig<Product, ProductDto>.NewConfig().TwoWays(); ;
        TypeAdapterConfig<ProductPropertyDto, ProductProperty>.NewConfig().TwoWays();

        TypeAdapterConfig<CreateProductSkuDto, ProductSku>.NewConfig();
        TypeAdapterConfig<ProductSku, ProductSkuDto>.NewConfig().TwoWays(); 

        TypeAdapterConfig<CreateImageDto, Image>.NewConfig();
        TypeAdapterConfig<Image, ImageDto>.NewConfig().TwoWays(); 

        TypeAdapterConfig<CreatePriceAdjustmentDto, PriceAdjustment>.NewConfig();
        TypeAdapterConfig<PriceAdjustment, PriceAdjustmentDto>.NewConfig().TwoWays(); 
        TypeAdapterConfig<CreatePriceAdjustmentItemDto, PriceAdjustmentItem>.NewConfig();
        TypeAdapterConfig<PriceAdjustmentItem, PriceAdjustmentItemDto>.NewConfig().TwoWays(); 
    }
}
