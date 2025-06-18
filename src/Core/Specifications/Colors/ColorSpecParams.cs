using Core.Common.Specifications;

namespace Core.Specifications.Colors
{
    public class ColorSpecParams : QueryStringParameter
    {
        public ColorFilterParameter Filter { get; set; } = new ColorFilterParameter();
    }
    public class ColorFilterParameter
    {
        public string? Name { get; set; }
        public string? HexCode { get; set; }
    }
}
