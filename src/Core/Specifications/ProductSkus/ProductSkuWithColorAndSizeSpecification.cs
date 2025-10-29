using Ardalis.Specification;
using Core.Entities;

namespace Core.Specifications.ProductSkus
{
    public class ProductSkuWithColorAndSizeSpecification : Specification<ProductSku>
    {
        public ProductSkuWithColorAndSizeSpecification()
        {
            Query
                .Include(x => x.Color)
                .Include(x => x.Size);
        }
        public ProductSkuWithColorAndSizeSpecification(Guid id)
        {
            Query.Where(x => x.Id == id)
                 .Include(x => x.Color)
                 .Include(x => x.Size);
        }


        public ProductSkuWithColorAndSizeSpecification(ProductSkuSpecParams specParams, bool isSearch = true)
        {
            Query.Where(x => x.ProductId == specParams.Filter.ProductId)
                 .Include(x => x.Color)
                 .Include(x => x.Size);
            if (isSearch)
            {
                Query.Skip(specParams.PageSize * (specParams.PageNumber - 1))
                     .Take(specParams.PageSize);
                switch (specParams.Sort)
                {
                    case "sku_code_asc":
                        Query.OrderBy(x => x.SkuCode);
                        break;
                    case "sku_code_desc":
                        Query.OrderByDescending(x => x.SkuCode);
                        break;
                    default:
                        Query.OrderByDescending(x => x.UpdatedAt!);
                        break;
                }
            }
        }
    }
}
