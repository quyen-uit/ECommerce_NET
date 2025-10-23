using Core.Common.Specifications;

namespace Core.Specifications.ProductSkus
{
    public class ProductSkuSpecParams : QueryStringParameter
    {
        public ProductSkusFilter Filter { get; set; } = new ProductSkusFilter();
    }

    public class ProductSkusFilter
    {
        public Guid? ProductId { get; set; }
    }
}
