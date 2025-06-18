using Core.Common.Entities;
using Core.Enums;

namespace Core.Entities.Inventory
{
    public class InventoryTransaction : AuditableEntity
    {
        public long ProductSkuId { get; set; }
        public InventoryTransactionType TransactionType { get; set; }
        public int Quantity { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        public string? Notes { get; set; }
        public long ReferenceId { get; set; }
        public decimal Price { get; set; }
        public ProductSku ProductSku { get; set; } = default!; // Navigation property
    }
}
