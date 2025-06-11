using Core.Common;

namespace Core.Entities
{
    public class ProductSku : AuditableEntity
    {
        public long ProductId { get; set; }
        public string SkuCode { get; set; }
        public bool IsActive { get; set; } = true;
        public long ColorId { get; set; }
        public long SizeId { get; set; }
        public Product Product { get; set; }
        public Color Color { get; set; }
        public Size Size { get; set; }
    }
}
