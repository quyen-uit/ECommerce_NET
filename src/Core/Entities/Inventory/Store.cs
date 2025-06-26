using Core.Common.Entities;

namespace Core.Entities.Inventory;
public class Store : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<StoreProductSku> StoreProductSkus { get; set; } = new List<StoreProductSku>();
}

