using Core.Common.Specifications;
using Core.Enums;

namespace Core.Specifications.Sizes
{
    public class SizeSpecParams : QueryStringParameter
    {
        public SizeFilter Filter { get; set; } = new SizeFilter();
    }
    public class SizeFilter
    {
        public string? Name { get; set; }
        public SizeType? SizeStype { get; set; }
        public FilterRange<int?> SortOrder { get; set; } = new FilterRange<int?>();
    }
}
