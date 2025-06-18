using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Specifications;

namespace Core.Specifications.ProductSkus
{
    public class ProductSkuSpecParams : QueryStringParameter
    {
        public ProductSkusFilter Filter { get; set; } = new ProductSkusFilter();
    }

    public class ProductSkusFilter
    {
        public long? ProductId { get; set; }
    }
}
