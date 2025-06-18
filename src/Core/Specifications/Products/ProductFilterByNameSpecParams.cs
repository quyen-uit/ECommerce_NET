using Core.Common.Specifications;

namespace Core.Specifications.Products
{
    public class ProductFilterByNameSpecParams : QueryStringParameter
    {
        public ProductFilterByName Filter { get; set; } = new ProductFilterByName();
    }
    public class ProductFilterByName
    {
        public string? CategoryName { get; set; }
        public string? ProductBrandName { get; set; }
        public string? Name { get; set; }
        public bool? IsNew { get; set; }
        public bool? IsTrending { get; set; }
        public bool? IsActive { get; set; }
    }
}
