using Core.Common.Entities;

namespace Core.Entities
{
    public class PriceAdjustmentItem : BaseEntity
    {
        public decimal SalePrice { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = default!; // Navigation property
        public Guid PriceAdjustmentId { get; set; }
        public PriceAdjustment PriceAdjustment { get; set; } = default!; // Navigation property
    }
}
