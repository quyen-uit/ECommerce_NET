using Core.Common.Entities;

namespace Core.Entities.Inventory;

public class StoreProductSku : AuditableEntity
{
    public long StoreId { get; set; }
    public Store Store { get; set; } = default!;

    public long ProductSkuId { get; set; }
    public ProductSku ProductSku { get; set; } = default!;
    public int Quantity { get; set; }
}


