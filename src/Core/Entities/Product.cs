using Core.Common;
using Core.Enums;

namespace Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PhotoUrl { get; set; }
        public int Stock { get; set; }
        public ProductSize Size { get; set; }
        public bool IsTrending { get; set; }
        public int PurchasedNumber { get; set; }

        public Category Category { get; set; }
        public int ProductTypeId { get; set; }
        public ProductBrand ProductBrand { get; set; }
        public int ProductBrandId { get; set; }
        public ICollection<ProductColor> ProductColors { get; set; }
        public ICollection<Review> Reviews { get; set; }

    }
}
