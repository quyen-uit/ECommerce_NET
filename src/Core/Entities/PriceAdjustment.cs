using Core.Common.Entities;

namespace Core.Entities
{
    public class PriceAdjustment : AuditableEntity
    {
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<PriceAdjustmentItem> PriceAdjustmentItems { get; set; } = new List<PriceAdjustmentItem>();
    }
}
