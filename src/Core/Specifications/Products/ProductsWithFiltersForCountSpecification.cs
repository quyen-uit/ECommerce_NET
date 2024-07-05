using Core.Common;
using Core.Entities;

namespace Core.Specifications.Products
{
    public class ProductsWithFiltersForCountSpecification : BaseSpecification<Product>
    {
        public ProductsWithFiltersForCountSpecification(ProductSpecParams productSpecParams)
            : base(x =>
            (string.IsNullOrEmpty(productSpecParams.Search) || x.Name.ToLower().Contains(productSpecParams.Search))
            && (!productSpecParams.BrandId.HasValue || productSpecParams.BrandId == x.ProductBrandId)
            && (!productSpecParams.TypeId.HasValue || productSpecParams.TypeId == x.ProductTypeId))

        {

        }

    }
}
