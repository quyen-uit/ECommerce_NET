using Core.Common;
using Core.Entities;

namespace Core.Specifications.Products
{
    public class ProductColorSpecification : BaseSpecification<ProductColor>
    {
        public ProductColorSpecification(int productId, int colorId) : base(x => x.ProductId == productId && x.ColorId == colorId)
        {
        }
    }
}
