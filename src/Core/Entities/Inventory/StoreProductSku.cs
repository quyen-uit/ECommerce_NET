using Core.Common.Entities;

namespace Core.Entities.Inventory;

public class StoreProductSku : AuditableEntity
{
    public Guid StoreId { get; set; }
    public Store Store { get; set; } = default!;

    public Guid ProductSkuId { get; set; }
    public ProductSku ProductSku { get; set; } = default!;
    public int Quantity { get; set; }
}


