using Core.Dtos;
using Core.Entities;
using Mapster;

public static class MapsterConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<CreateColorDto, Color>.NewConfig();
        TypeAdapterConfig<UpdateColorDto, Color>.NewConfig();
        TypeAdapterConfig<Color, ColorDto>.NewConfig();

        TypeAdapterConfig<CreateSizeDto, Size>.NewConfig();
        TypeAdapterConfig<UpdateSizeDto, Size>.NewConfig();
        TypeAdapterConfig<Size, SizeDto>.NewConfig();

        TypeAdapterConfig<CreateCategoryDto, Category>.NewConfig();
        TypeAdapterConfig<UpdateCategoryDto, Category>.NewConfig();
        TypeAdapterConfig<Category, CategoryDto>.NewConfig();

        TypeAdapterConfig<ProductBrandDto, ProductBrand>.NewConfig();
        TypeAdapterConfig<UpdateProductBrandDto, ProductBrand>.NewConfig();
        TypeAdapterConfig<ProductBrand, ProductBrandDto>.NewConfig();
    }
}
