using Core.Dtos.Categories;
using Core.Dtos.Colors;
using Core.Dtos.Images;
using Core.Dtos.PriceAdjustments;
using Core.Dtos.ProductBrands;
using Core.Dtos.Products;
using Core.Dtos.ProductSkus;
using Core.Dtos.Sizes;
using Core.Entities;
using Core.Enums;
using Mapster;

public static class MapsterConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<CreateColorDto, Color>.NewConfig();
        TypeAdapterConfig<Color, ColorDto>.NewConfig().TwoWays();

        TypeAdapterConfig<CreateSizeDto, Size>.NewConfig();
        TypeAdapterConfig<Size, SizeDto>.NewConfig().Map(dest => dest.SizeType, src => src.SizeType.ToString());

        TypeAdapterConfig<CreateCategoryDto, Category>.NewConfig();
        TypeAdapterConfig<Category, CategoryDto>.NewConfig().TwoWays();

        TypeAdapterConfig<CreateProductBrandDto, ProductBrand>.NewConfig();
        TypeAdapterConfig<ProductBrand, ProductBrandDto>.NewConfig().TwoWays();

        TypeAdapterConfig<CreateProductDto, Product>.NewConfig();
        TypeAdapterConfig<Product, ProductDto>.NewConfig().TwoWays();
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

    private static SizeType ParseSizeType(string sizeType)
    {
        if (Enum.TryParse<SizeType>(sizeType, true, out var type))
        {
            return type;
        }
        throw new ArgumentException("Invalid enum value");
    }
}
