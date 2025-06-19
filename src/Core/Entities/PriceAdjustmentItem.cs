using Core.Common.Entities;

namespace Core.Entities
{
    public class PriceAdjustmentItem : BaseEntity
    {
        public decimal SalePrice { get; set; }
        public long ProductId { get; set; }
        public Product Product { get; set; } = default!; // Navigation property
        public long PriceAdjustmentId { get; set; }
        public PriceAdjustment PriceAdjustment { get; set; } = default!; // Navigation property
    }
}
