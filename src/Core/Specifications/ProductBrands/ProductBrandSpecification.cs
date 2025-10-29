using Ardalis.Specification;
using Core.Entities;

namespace Core.Specifications.ProductBrands
{
    public class ProductBrandSpecification : Specification<ProductBrand>
    {
        public ProductBrandSpecification() { }

        public ProductBrandSpecification(ProductBrandSpecParams specParams)
        {
            Query.Where(x => string.IsNullOrEmpty(specParams.Filter.Name)
                              || specParams.Filter.Name.ToLower().Contains(x.Name.ToLower()));
            Query.Skip(specParams.PageSize * (specParams.PageNumber - 1))
                 .Take(specParams.PageSize);
            switch (specParams.Sort)
            {
                case "name_asc":
                    Query.OrderBy(x => x.Name);
                    break;
                case "name_desc":
                    Query.OrderByDescending(x => x.Name);
                    break;
                default:
                    Query.OrderBy(x => x.Name);
                    break;
            }
        }
    }
}
