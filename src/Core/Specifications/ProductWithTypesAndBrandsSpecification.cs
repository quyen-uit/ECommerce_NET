using Core.Common;
using Core.Entities;
using Core.Specifications.Parameters;

namespace Core.Specifications
{
    public class ProductWithTypesAndBrandsSpecification : BaseSpecification<Product>
    {
        public ProductWithTypesAndBrandsSpecification()
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
        }

        public ProductWithTypesAndBrandsSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
        }

        public ProductWithTypesAndBrandsSpecification(ProductSpecParams productSpecParams) 
            : base(x =>
            (string.IsNullOrEmpty(productSpecParams.Search) || x.Name.ToLower().Contains(productSpecParams.Search))
            && (!productSpecParams.BrandId.HasValue || productSpecParams.BrandId == x.ProductBrandId) 
            && (!productSpecParams.BrandId.HasValue || productSpecParams.BrandId == x.ProductTypeId))
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
            AddPagination(productSpecParams.PageSize * (productSpecParams.PageIndex - 1), productSpecParams.PageSize);

            switch (productSpecParams.Sort)
            {
                case "priceAsc":
                    AddOrderBy(x => x.Price);
                    break;
                case "priceDesc":
                    AddOrderByDescending(x => x.Price);
                    break;
                default:
                    AddOrderBy(x => x.Name);
                    break;
            }
        }
    }
}
