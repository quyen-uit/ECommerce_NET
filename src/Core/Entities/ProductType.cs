using Core.Common;

namespace Core.Entities
{
    public class ProductType:BaseEntity
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public ICollection<Product> Products { get; set; }

    }
} 