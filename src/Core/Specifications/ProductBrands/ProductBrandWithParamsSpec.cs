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
    public class ProductBrandWithParamsSpec : BaseSpecification<ProductBrand>
    {
        public ProductBrandWithParamsSpec() { }

        public ProductBrandWithParamsSpec(ProductBrandSpecParams specParams)
            : base(x =>
                (
                    string.IsNullOrEmpty(specParams.Search)
                    || x.Name.ToLower().Contains(specParams.Search)
                )
            )
        {
            AddPagination(specParams.PageSize, specParams.PageNumber);
            switch (specParams.Sort)
            {
                case "name_asc":
                    AddOrderBy(x => x.Name);
                    break;
                case "name_desc":
                    AddOrderByDescending(x => x.Name);
                    break;
                default:
                    AddOrderBy(x => x.Name);
                    break;
            }
        }
    }
}
