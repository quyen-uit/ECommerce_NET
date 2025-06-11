using Core.Common;

namespace Core.Specifications.Colors
{
    public class ColorSpecParams : QueryStringParameter
    {
        public string Sort { get; set; }
        private string _search;
        public string Search
        {
            get => _search;
            set => _search = value != null ? value.ToLower() : "";
        }
    }
}
