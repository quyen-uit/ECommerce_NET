using Core.Common;

namespace Core.Entities.ReturnOrder
{
    public class ReturnOrderItem : AuditableEntity
    {
        public long ReturnOrderId { get; set; }
        public long ProductSkuId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public ReturnOrder ReturnOrder { get; set; }
        public ProductSku ProductSku { get; set; }
    }
}
