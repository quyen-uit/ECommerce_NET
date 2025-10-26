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

        public ProductWithTypesAndBrandsSpecification(ProductSpecParams productSpecParams, bool isSearch = true)
            : base(x =>
            (string.IsNullOrEmpty(productSpecParams.Filter.Name) || x.Name.ToLower().Contains(productSpecParams.Filter.Name.ToLower()))
            && (!productSpecParams.Filter.ProductBrandId.HasValue || x.ProductBrandId == productSpecParams.Filter.ProductBrandId)
            && (!productSpecParams.Filter.CategoryId.HasValue || x.CategoryId == productSpecParams.Filter.CategoryId)
            && (!productSpecParams.Filter.IsNew.HasValue || x.IsNew == productSpecParams.Filter.IsNew.Value)
            && (!productSpecParams.Filter.IsTrending.HasValue || x.IsTrending == productSpecParams.Filter.IsTrending.Value)
            )
        {
            AddInclude(x => x.Category);
            AddInclude(x => x.ProductBrand);

            if (isSearch)
            {
                AddPagination(productSpecParams.PageSize, productSpecParams.PageNumber);
                AddSorting(productSpecParams.Sort);
            }
        }

        public ProductWithTypesAndBrandsSpecification(ProductFilterByNameSpecParams productSpecParams, bool isSearch = true)
            : base(x =>
            (string.IsNullOrEmpty(productSpecParams.Filter.Name) || x.Name.ToLower().Contains(productSpecParams.Filter.Name.ToLower()))
            && (string.IsNullOrEmpty(productSpecParams.Filter.ProductBrandName) || x.ProductBrand.Name.ToLower().Contains(productSpecParams.Filter.ProductBrandName.ToLower()))
            && (string.IsNullOrEmpty(productSpecParams.Filter.CategoryName) || x.Category.Name.ToLower().Contains(productSpecParams.Filter.CategoryName.ToLower()))
            && (!productSpecParams.Filter.IsNew.HasValue || x.IsNew == productSpecParams.Filter.IsNew.Value)
            && (!productSpecParams.Filter.IsTrending.HasValue || x.IsTrending == productSpecParams.Filter.IsTrending.Value)
            && (!productSpecParams.Filter.IsActive.HasValue || x.IsActive == productSpecParams.Filter.IsActive.Value)
            )
        {
            AddInclude(x => x.Category);
            AddInclude(x => x.ProductBrand);

            if (isSearch)
            {
                AddPagination(productSpecParams.PageSize, productSpecParams.PageNumber);
                AddSorting(productSpecParams.Sort);
            }
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
