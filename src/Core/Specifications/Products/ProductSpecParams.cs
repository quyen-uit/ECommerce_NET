using Core.Common.Specifications;

namespace Core.Specifications.Products
{
    public class ProductSpecParams : QueryStringParameter
    {
        public ProductFilter Filter { get; set; } = new ProductFilter();
    }
    public class ProductFilter
    {
        public int? CategoryId { get; set; }
        public int? ProductBrandId { get; set; }
        public string? Name { get; set; }
        public bool? IsNew { get; set; }
        public bool? IsTrending { get; set; }
    }
}
