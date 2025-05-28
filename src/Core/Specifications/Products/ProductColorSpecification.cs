using Core.Common;
using Core.Entities;

namespace Core.Specifications.Products
{
    public class ProductColorSpecification : BaseSpecification<ProductColor>
    {
        public ProductColorSpecification(long productId, long colorId) : base(x => x.ProductId == productId && x.ColorId == colorId)
        {
        }
    }
}
