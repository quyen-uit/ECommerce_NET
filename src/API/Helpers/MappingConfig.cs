using Core.Dtos;
using Core.Entities;
using Mapster;

public static class MapsterConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<CreateColorDto, Color>.NewConfig();
        TypeAdapterConfig<UpdateColorDto, Color>.NewConfig();
        TypeAdapterConfig<Color, ColorDto>.NewConfig().TwoWays();

        TypeAdapterConfig<CreateSizeDto, Size>.NewConfig();
        TypeAdapterConfig<UpdateSizeDto, Size>.NewConfig();
        TypeAdapterConfig<Size, SizeDto>.NewConfig();

        TypeAdapterConfig<CreateCategoryDto, Category>.NewConfig();
        TypeAdapterConfig<UpdateCategoryDto, Category>.NewConfig();
        TypeAdapterConfig<Category, CategoryDto>.NewConfig().TwoWays();

        TypeAdapterConfig<CreateProductBrandDto, ProductBrand>.NewConfig();
        TypeAdapterConfig<UpdateProductBrandDto, ProductBrand>.NewConfig();
        TypeAdapterConfig<ProductBrand, ProductBrandDto>.NewConfig().TwoWays();

        TypeAdapterConfig<CreateProductDto, Product>.NewConfig();
        TypeAdapterConfig<UpdateProductDto, Product>.NewConfig();
        TypeAdapterConfig<Product, ProductDto>.NewConfig().TwoWays();;
        TypeAdapterConfig<ProductPropertyDto, ProductProperty>.NewConfig().TwoWays();

        TypeAdapterConfig<CreateProductSkuDto, ProductSku>.NewConfig();
        TypeAdapterConfig<UpdateProductSkuDto, ProductSku>.NewConfig();
        TypeAdapterConfig<ProductSku, ProductSkuDto>.NewConfig().TwoWays(); ;

    }
}
