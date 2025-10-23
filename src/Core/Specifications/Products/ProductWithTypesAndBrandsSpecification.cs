using Core.Common.Specifications;
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

        public ProductWithTypesAndBrandsSpecification(Guid id) : base(x => x.Id == id)
        {
            AddInclude(x => x.Category);
            AddInclude(x => x.ProductBrand);
        }

        public ProductWithTypesAndBrandsSpecification(ProductSpecParams productSpecParams)
            : base(x =>
            (string.IsNullOrEmpty(productSpecParams.Filter.Name) || productSpecParams.Filter.Name.ToLower().Contains(x.Name.ToLower()))
            && (!productSpecParams.Filter.ProductBrandId.HasValue || productSpecParams.Filter.ProductBrandId == x.ProductBrandId)
            && (!productSpecParams.Filter.CategoryId.HasValue || productSpecParams.Filter.CategoryId == x.CategoryId)
            && (!productSpecParams.Filter.IsNew.HasValue || productSpecParams.Filter.IsNew == true)
            && (!productSpecParams.Filter.IsTrending.HasValue || productSpecParams.Filter.IsTrending == true)
            )
        {
            AddInclude(x => x.Category);
            AddInclude(x => x.ProductBrand);

            AddPagination(productSpecParams.PageSize, productSpecParams.PageNumber);

            AddSorting(productSpecParams.Sort);
        }

        public ProductWithTypesAndBrandsSpecification(ProductFilterByNameSpecParams productSpecParams)
            : base(x =>
            (string.IsNullOrEmpty(productSpecParams.Filter.Name) || x.Name.ToLower().Contains(productSpecParams.Filter.Name.ToLower()))
            && (string.IsNullOrEmpty(productSpecParams.Filter.ProductBrandName) || x.Name.ToLower().Contains(productSpecParams.Filter.ProductBrandName.ToLower()))
            && (string.IsNullOrEmpty(productSpecParams.Filter.CategoryName) || x.Name.ToLower().Contains(productSpecParams.Filter.CategoryName.ToLower()))
            && (!productSpecParams.Filter.IsNew.HasValue || productSpecParams.Filter.IsNew == true)
            && (!productSpecParams.Filter.IsTrending.HasValue || productSpecParams.Filter.IsTrending == true)
            && (!productSpecParams.Filter.IsActive.HasValue || productSpecParams.Filter.IsActive == true)
            )
        {
            AddInclude(x => x.Category);
            AddInclude(x => x.ProductBrand);

            AddPagination(productSpecParams.PageSize, productSpecParams.PageNumber);

            AddSorting(productSpecParams.Sort);
        }

        private void AddSorting(string? sort)
        {
            switch (sort)
            {
                case "name_asc":
                    AddOrderBy(x => x.Name);
                    break;
                case "name_desc":
                    AddOrderByDescending(x => x.Name);
                    break;
                case "product_brand_name_asc":
                    AddOrderBy(x => x.ProductBrand.Name);
                    break;
                case "product_brand_name_desc":
                    AddOrderByDescending(x => x.ProductBrand.Name);
                    break;
                case "category_name_asc":
                    AddOrderBy(x => x.Category.Name);
                    break;
                case "category_name_desc":
                    AddOrderByDescending(x => x.Category.Name);
                    break;
                default:
                    AddOrderByDescending(x => x.UpdatedAt!);
                    break;
            }
        }
    }
}
