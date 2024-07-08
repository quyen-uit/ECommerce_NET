using Core.Common;

namespace Core.Specifications.Products
{
    public class ProductSpecParams : QueryStringParameter
    {
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
        public bool? IsNew { get; set; }
        public bool? IsTrending { get; set; }
        public string Sort { get; set; }
        private string _search;
        public string Search
        {
            get => _search;
            set => _search = value.ToLower();
        }
    }
}
