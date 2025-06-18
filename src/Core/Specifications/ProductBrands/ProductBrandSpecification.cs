using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Core.Common.Specifications;
using Core.Entities;
using Core.Specifications.Products;

namespace Core.Specifications.ProductBrands
{
    public class ProductBrandSpecification : BaseSpecification<ProductBrand>
    {
        public ProductBrandSpecification() { }

        public ProductBrandSpecification(ProductBrandSpecParams specParams)
            : base(x =>
                (
                    string.IsNullOrEmpty(specParams.Filter.Name)
                    || x.Name.ToLower().Contains(specParams.Filter.Name.ToLower())
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
