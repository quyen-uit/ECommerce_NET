using Core.Common;
using Core.Entities;
using Core.Specifications.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Core.Specifications.Categories
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
