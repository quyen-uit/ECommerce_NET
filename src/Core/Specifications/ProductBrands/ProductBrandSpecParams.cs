using Core.Common.Specifications;

namespace Core.Specifications.ProductBrands
{
    public class ProductBrandSpecParams : QueryStringParameter
    {
        public ProductBrandFilter Filter { get; set; } = new ProductBrandFilter();
    }

    public class ProductBrandFilter
    {
        public string? Name { get; set; }
    }
}
