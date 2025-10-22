namespace Core.Dtos.PriceAdjustments
{
    public class PriceAdjustmentDto
    {
        public required string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<PriceAdjustmentItemDto> PriceAdjustmentItems { get; set; } = new List<PriceAdjustmentItemDto>();
    }

}
