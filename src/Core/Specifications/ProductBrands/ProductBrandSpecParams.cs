using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common;

namespace Core.Specifications.ProductBrands
{
    public class ProductBrandSpecParams : QueryStringParameter
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
