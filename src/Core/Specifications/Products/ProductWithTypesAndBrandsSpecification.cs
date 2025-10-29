using Ardalis.Specification;
using Core.Entities;

namespace Core.Specifications.Products
{
    public class ProductWithTypesAndBrandsSpecification : Specification<Product>
    {
        public ProductWithTypesAndBrandsSpecification()
        {
            Query
                .Include(x => x.Category)
                .Include(x => x.ProductBrand);
        }

        public ProductWithTypesAndBrandsSpecification(Guid id)
        {
            Query
                .Where(x => x.Id == id)
                .Include(x => x.Category)
                .Include(x => x.ProductBrand);
        }

        public ProductWithTypesAndBrandsSpecification(ProductSpecParams productSpecParams, bool isSearch = true)
        {
            Query
                .Where(x =>
                    (string.IsNullOrEmpty(productSpecParams.Filter.Name) || x.Name.ToLower().Contains(productSpecParams.Filter.Name.ToLower()))
                    && (!productSpecParams.Filter.ProductBrandId.HasValue || x.ProductBrandId == productSpecParams.Filter.ProductBrandId)
                    && (!productSpecParams.Filter.CategoryId.HasValue || x.CategoryId == productSpecParams.Filter.CategoryId)
                    && (!productSpecParams.Filter.IsNew.HasValue || x.IsNew == productSpecParams.Filter.IsNew.Value)
                    && (!productSpecParams.Filter.IsTrending.HasValue || x.IsTrending == productSpecParams.Filter.IsTrending.Value)
                )
                .Include(x => x.Category)
                .Include(x => x.ProductBrand);

            if (isSearch)
            {
                Query.Skip(productSpecParams.PageSize * (productSpecParams.PageNumber - 1))
                     .Take(productSpecParams.PageSize);
                AddSorting(productSpecParams.Sort);
            }
        }

        public ProductWithTypesAndBrandsSpecification(ProductFilterByNameSpecParams productSpecParams, bool isSearch = true)
        {
            Query
                .Where(x =>
                    (string.IsNullOrEmpty(productSpecParams.Filter.Name) || x.Name.ToLower().Contains(productSpecParams.Filter.Name.ToLower()))
                    && (string.IsNullOrEmpty(productSpecParams.Filter.ProductBrandName) || x.ProductBrand.Name.ToLower().Contains(productSpecParams.Filter.ProductBrandName.ToLower()))
                    && (string.IsNullOrEmpty(productSpecParams.Filter.CategoryName) || x.Category.Name.ToLower().Contains(productSpecParams.Filter.CategoryName.ToLower()))
                    && (!productSpecParams.Filter.IsNew.HasValue || x.IsNew == productSpecParams.Filter.IsNew.Value)
                    && (!productSpecParams.Filter.IsTrending.HasValue || x.IsTrending == productSpecParams.Filter.IsTrending.Value)
                    && (!productSpecParams.Filter.IsActive.HasValue || x.IsActive == productSpecParams.Filter.IsActive.Value)
                )
                .Include(x => x.Category)
                .Include(x => x.ProductBrand);

            if (isSearch)
            {
                Query.Skip(productSpecParams.PageSize * (productSpecParams.PageNumber - 1))
                     .Take(productSpecParams.PageSize);
                AddSorting(productSpecParams.Sort);
            }
        }

        private void AddSorting(string? sort)
        {
            switch (sort)
            {
                case "name_asc":
                    Query.OrderBy(x => x.Name);
                    break;
                case "name_desc":
                    Query.OrderByDescending(x => x.Name);
                    break;
                case "product_brand_name_asc":
                    Query.OrderBy(x => x.ProductBrand.Name);
                    break;
                case "product_brand_name_desc":
                    Query.OrderByDescending(x => x.ProductBrand.Name);
                    break;
                case "category_name_asc":
                    Query.OrderBy(x => x.Category.Name);
                    break;
                case "category_name_desc":
                    Query.OrderByDescending(x => x.Category.Name);
                    break;
                default:
                    Query.OrderByDescending(x => x.UpdatedAt!);
                    break;
            }
        }
    }
}
