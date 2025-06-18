using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Specifications;

namespace Core.Specifications.ProductBrands
{
    public class ProductBrandSpecParams : QueryStringParameter
    {
        public ProductBrandFilter Filter { get; set; } = new ProductBrandFilter();
    }

    public class ProductBrandFilter
    {
        public string? Name { get; set; }
    }
}
