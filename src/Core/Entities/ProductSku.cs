using Core.Common.Entities;
using Core.Entities.Inventory;

namespace Core.Entities
{
    public class ProductSku : AuditableEntity
    {
        public Guid ProductId { get; set; }
        public string SkuCode { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public Guid ColorId { get; set; }
        public Guid SizeId { get; set; }
        public Product Product { get; set; } = default!;
        public Color Color { get; set; } = default!;
        public Size Size { get; set; } = default!;
        public ICollection<StoreProductSku> StoreProductSkus { get; set; } = new List<StoreProductSku>();
    }
}
