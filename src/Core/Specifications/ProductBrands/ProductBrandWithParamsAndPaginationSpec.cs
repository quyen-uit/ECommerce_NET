using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Core.Common;
using Core.Entities;
using Core.Specifications.Products;

namespace Core.Specifications.ProductBrands
{
    public class ProductBrandWithParamsAndPaginationSpec : ProductBrandWithParamsSpec
    {
        public ProductBrandWithParamsAndPaginationSpec() { }

        public ProductBrandWithParamsAndPaginationSpec(ProductBrandSpecParams specParams)
            : base(specParams)
        {
            AddPagination(specParams.PageSize, specParams.PageNumber);
        }
    }
}
