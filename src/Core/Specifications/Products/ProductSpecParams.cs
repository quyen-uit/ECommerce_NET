using Core.Common.Specifications;

namespace Core.Specifications.Products
{
    public class ProductSpecParams : QueryStringParameter
    {
        public ProductFilter Filter { get; set; } = new ProductFilter();
    }
    public class ProductFilter
    {
        public Guid? CategoryId { get; set; }
        public Guid? ProductBrandId { get; set; }
        public string? Name { get; set; }
        public bool? IsNew { get; set; }
        public bool? IsTrending { get; set; }
    }
}
