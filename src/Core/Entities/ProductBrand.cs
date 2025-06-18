using Core.Common.Entities;

namespace Core.Entities
{
    public class ProductBrand : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
    }
}