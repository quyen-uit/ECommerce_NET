using Core.Enums;

namespace Core.Dtos.Products
{
    public class CreateProductDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
        public bool IsTrending { get; set; }
        public bool IsNew { get; set; }
        public bool IsActive { get; set; } = true;
        public GenderType GenderType { get; set; }
        public Guid CategoryId { get; set; }
        public Guid ProductBrandId { get; set; }
        public ICollection<ProductPropertyDto> Properties { get; set; } = new List<ProductPropertyDto>();
    }
}
