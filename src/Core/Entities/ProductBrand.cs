using Core.Common;

namespace Core.Entities
{
    public class ProductBrand : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string LogoUrl { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}