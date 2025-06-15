using Core.Common;
using Core.Entities;
using Core.Enums;

namespace Core.Entities
{
    public class Product : AuditableEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
        public bool IsTrending { get; set; }
        public bool IsNew { get; set; }
        public bool IsActive { get; set; } = true;
        public GenderType GenderType { get; set; }

        public Category Category { get; set; } = default!;
        public long CategoryId { get; set; }
        public ProductBrand ProductBrand { get; set; } = default!;
        public long ProductBrandId { get; set; }
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<ProductSku> ProductSkus { get; set; } = new List<ProductSku>();
        public ICollection<ProductProperty> Properties { get; set; } = new List<ProductProperty>();
        public ICollection<Collection> Collections { get; set; } = new List<Collection>();
        public int Price { get; set; }
        public List<ProductSize> Size { get; set; } = new List<ProductSize>();

    }

}

public class ProductProperty
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
