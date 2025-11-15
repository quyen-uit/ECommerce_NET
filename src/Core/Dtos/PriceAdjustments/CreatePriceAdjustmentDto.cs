namespace Core.Dtos.PriceAdjustments
{
    public class CreatePriceAdjustmentDto
    {
        public Guid? Id { get; set; }
        public Guid ProductSkuId { get; set; }
        public decimal SalePrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<CreatePriceAdjustmentItemDto> PriceAdjustmentItems { get; set; } = new List<CreatePriceAdjustmentItemDto>();
    }
}
