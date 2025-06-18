using Core.Common.Entities;

namespace Core.Entities.Inventory
{
    public class InventoryAdjustment : AuditableEntity
    {
        public long ProductSkuId { get; set; }
        public int Quantity { get; set; }
        public DateTime AdjustmentDate { get; set; } = DateTime.UtcNow;
        public string? Reason { get; set; } 
        public decimal Price { get; set; }
        public ProductSku ProductSku { get; set; } = default!; // Navigation property
    }
}
