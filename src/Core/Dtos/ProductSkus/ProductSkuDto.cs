using Core.Dtos.Colors;
using Core.Dtos.Images;
using Core.Dtos.Sizes;

namespace Core.Dtos.ProductSkus
{
    public class ProductSkuDto
    {
        public Guid Id { get; set; }
        public required string SkuCode { get; set; }
        public bool IsActive { get; set; } = true;
        public int Quantity { get; set; }
        public ColorDto? Color { get; set; } = default!;
        public SizeDto? Size { get; set; } = default!;
        public ICollection<ImageDto> Images { get; set; } = new List<ImageDto>();
    }

}
