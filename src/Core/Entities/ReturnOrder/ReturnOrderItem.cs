using Core.Common.Entities;

namespace Core.Entities.ReturnOrder
{
    public class ReturnOrderItem : AuditableEntity
    {
        public Guid ReturnOrderId { get; set; }
        public Guid ProductSkuId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public ReturnOrder ReturnOrder { get; set; } = default!; // Navigation property
        public ProductSku ProductSku { get; set; } = default!;
    }
}
