using Core.Common.Entities;
using Core.Enums;

namespace Core.Entities.Inventory
{
    public class InventoryTransaction : AuditableEntity
    {
        public Guid ProductSkuId { get; set; }
        public InventoryTransactionType TransactionType { get; set; }
        public int Quantity { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        public string? Notes { get; set; }
        public Guid ReferenceId { get; set; }
        public decimal Price { get; set; }
        public ProductSku ProductSku { get; set; } = default!; // Navigation property
    }
}
