using Core.Common;

namespace Core.Specifications.Sizes
{
    public class SizeSpecParams : QueryStringParameter
    {
        public string Search { get; set; }
        public string Sort { get; set; }
    }
}
