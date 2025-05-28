using Core.Common;

namespace Core.Entities
{
    public class PriceAdjustment : BaseEntity
    {
        public long ProductSkuId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal SalePrice { get; set; }
        public ProductSku ProductSku { get; set; }
    }
}
