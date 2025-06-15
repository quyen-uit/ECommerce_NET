using Core.Common;

namespace Core.Specifications.Colors
{
    public class ColorSpecParams : QueryStringParameter
    {
        public string? Sort { get; set; }
        public ColorFilterParameter Filter { get; set; } = null!;
        private string _search = string.Empty;
        public string Search
        {
            get => _search;
            set => _search = value != null ? value.ToLower() : "";
        }
    }
    public class ColorFilterParameter
    {
        public string? Name { get; set; }
        public string? HexCode { get; set; }
    }
}
