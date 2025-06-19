using Core.Common.Specifications;

namespace Core.Specifications.PriceAdjustments
{
    public class PriceAdjustmentSpecParams : QueryStringParameter
    {
        public PriceAdjustmentFilter Filter { get; set; } = new PriceAdjustmentFilter();
    }
    public class PriceAdjustmentFilter
    {
        public string? Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
