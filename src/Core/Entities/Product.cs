using Core.Common;
using Core.Entities;
using Core.Enums;

namespace Core.Entities
{
    public class Product : AuditableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public bool IsTrending { get; set; }
        public bool IsNew { get; set; }
        public bool IsActive { get; set; } = true;
        public GenderType GenderType { get; set; }

        public Category Category { get; set; }
        public long CategoryId { get; set; }
        public ProductBrand ProductBrand { get; set; }
        public long ProductBrandId { get; set; }
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<ProductSku> ProductSkus { get; set; } = new List<ProductSku>();
        public ICollection<ProductProperty> Properties { get; set; }
        public ICollection<Collection> Collections { get; set; }
        public int Price { get; set; }
        public List<ProductSize> Size { get; set; }

    }

}

public class ProductProperty
{
    public string Key { get; set; }
    public string Value { get; set; }
}
