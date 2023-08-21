using Core.Common;
using Core.Entities;
using Core.Specifications.Parameters;

namespace Core.Specifications
{
    public class ProductsWithFiltersForCountSpecification : BaseSpecification<Product>
    {
        public ProductsWithFiltersForCountSpecification(ProductSpecParams productSpecParams)
            : base(x =>
            (string.IsNullOrEmpty(productSpecParams.Search) || x.Name.ToLower().Contains(productSpecParams.Search))
            && (!productSpecParams.BrandId.HasValue || productSpecParams.BrandId == x.ProductBrandId)
            && (!productSpecParams.BrandId.HasValue || productSpecParams.BrandId == x.ProductTypeId))

        {

        }

    }
}
