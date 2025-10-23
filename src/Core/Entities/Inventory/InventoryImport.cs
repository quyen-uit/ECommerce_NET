using Core.Common.Entities;

namespace Core.Entities.Inventory
{
    public class InventoryImport : AuditableEntity
    {
        public Guid ProductSkuId { get; set; }
        public int Quantity { get; set; }
        public DateTime ImportDate { get; set; } = DateTime.UtcNow;
        public string? Notes { get; set; }
        public Guid VendorId { get; set; }
        public decimal Price { get; set; }
        public ProductSku ProductSku { get; set; } = default!;
        public Vendor Vendor { get; set; } = default!;
    }
}
