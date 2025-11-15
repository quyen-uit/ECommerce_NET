using Core.Exceptions;

namespace Core.Dtos.ProductSkus
{
    public class CreateProductSkuDto
    {
        public Guid? Id { get; set; }
        public string SkuCode { get; set; } = string.Empty;
        public Guid ProductId { get; set; }
        public Guid ColorId { get; set; }
        public Guid SizeId { get; set; }
        public bool IsActive { get; set; } = true;
        // public ICollection<CreateOrUpdateImageDto> Images { get; set; } = new List<CreateOrUpdateImageDto>();

    }

}
