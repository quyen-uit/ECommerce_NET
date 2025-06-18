using Core.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Inventory
{
    public class InventoryImport : AuditableEntity
    {
        public long ProductSkuId { get; set; }
        public int Quantity { get; set; }
        public DateTime ImportDate { get; set; } = DateTime.UtcNow;
        public string? Notes { get; set; }
        public long VendorId { get; set; }
        public decimal Price { get; set; }
        public ProductSku ProductSku { get; set; } = default!;
        public Vendor Vendor { get; set; } = default!;
    }
}
