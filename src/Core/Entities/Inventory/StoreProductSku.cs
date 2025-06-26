using Core.Common.Entities;

namespace Core.Entities.Inventory;

public class StoreProductSku : AuditableEntity
{
    public int StoreId { get; set; }
    public Store Store { get; set; } = default!;

    public int ProductSkuId { get; set; }
    public ProductSku ProductSku { get; set; } = default!;
    public int Quantity { get; set; }
}


