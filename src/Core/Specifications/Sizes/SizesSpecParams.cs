using Core.Common.Specifications;

namespace Core.Specifications.Sizes
{
    public class SizeSpecParams : QueryStringParameter
    {
        public SizeFilter Filter { get; set; } = new SizeFilter();
    }
    public class SizeFilter
    {
        public string? Name { get; set; }
    }
}
