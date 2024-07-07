using Core.Common;
using Core.Entities;

namespace Core.Specifications.Products
{
    public class ProductWithTypesAndBrandsSpecification : BaseSpecification<Product>
    {
        public ProductWithTypesAndBrandsSpecification()
        {
            AddInclude(x => x.Category);
            AddInclude(x => x.ProductBrand);
        }

        public ProductWithTypesAndBrandsSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.Category);
            AddInclude(x => x.ProductBrand);
            AddInclude(x => x.ProductColors);
        }

        public ProductWithTypesAndBrandsSpecification(ProductSpecParams productSpecParams)
            : base(x =>
            (string.IsNullOrEmpty(productSpecParams.Search) || x.Name.ToLower().Contains(productSpecParams.Search))
            && (!productSpecParams.BrandId.HasValue || productSpecParams.BrandId == x.ProductBrandId)
            && (!productSpecParams.TypeId.HasValue || productSpecParams.TypeId == x.CategoryId))
        {
            AddInclude(x => x.Category);
            AddInclude(x => x.ProductBrand);
            AddInclude(x => x.ProductColors);
            AddPagination(productSpecParams.PageSize * (productSpecParams.PageNumber - 1), productSpecParams.PageSize);

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
