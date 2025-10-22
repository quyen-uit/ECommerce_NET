using Core.Common.Specifications;
using Core.Entities;

namespace Core.Specifications.ProductBrands
{
    public class ProductBrandSpecification : BaseSpecification<ProductBrand>
    {
        public ProductBrandSpecification() { }

        public ProductBrandSpecification(ProductBrandSpecParams specParams)
            : base(x =>
                (
                    string.IsNullOrEmpty(specParams.Filter.Name)
                    || specParams.Filter.Name.ToLower().Contains(x.Name.ToLower())
                )
            )
        {
            AddPagination(specParams.PageSize, specParams.PageNumber);
            switch (specParams.Sort)
            {
                case "name_asc":
                    AddOrderBy(x => x.Name);
                    break;
                case "name_desc":
                    AddOrderByDescending(x => x.Name);
                    break;
                default:
                    AddOrderBy(x => x.Name);
                    break;
            }
        }
    }
}
