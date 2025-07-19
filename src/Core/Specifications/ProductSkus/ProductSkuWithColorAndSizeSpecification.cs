using Core.Common.Specifications;
using Core.Entities;

namespace Core.Specifications.ProductSkus
{
    public class ProductSkuWithColorAndSizeSpecification : BaseSpecification<ProductSku>
    {
        public ProductSkuWithColorAndSizeSpecification()
        {
            AddInclude(x => x.Color);
            AddInclude(x => x.Size);
        }
        public ProductSkuWithColorAndSizeSpecification(long id): base(x => x.Id == id)
        {
            AddInclude(x => x.Color);
            AddInclude(x => x.Size);
        }


        public ProductSkuWithColorAndSizeSpecification(ProductSkuSpecParams specParams)
            : base(x => (x.ProductId == specParams.Filter.ProductId))
        {
            AddInclude(x => x.Color);
            AddInclude(x => x.Size);
            switch (specParams.Sort)
            {
                case "sku_code_asc":
                    AddOrderBy(x => x.SkuCode);
                    break;
                case "sku_code_desc":
                    AddOrderByDescending(x => x.SkuCode);
                    break;
                default:
                    AddOrderByDescending(x => x.UpdatedAt!);
                    break;
            }
        }
    }
}
