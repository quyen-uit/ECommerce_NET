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
    public class CategoryWithParamsSpec : BaseSpecification<Category>
    {
        public CategoryWithParamsSpec() { }
        public CategoryWithParamsSpec(CategorySpecParams specParams)
            : base(x => (string.IsNullOrEmpty(specParams.Search) || x.Name.ToLower().Contains(specParams.Search))
                     && (!specParams.IsActive.HasValue || x.IsActive == specParams.IsActive))
        {
            switch (specParams.Sort)
            {
                case "name_asc":
                    AddOrderBy(x => x.Name);
                    break;
                case "name_desc":
                    AddOrderByDescending(x => x.Name);
                    break;
                case "order_asc":
                    AddOrderBy(x => x.Order);
                    break;
                case "order_desc":
                    AddOrderByDescending(x => x.Order);
                    break;
                default:
                    AddOrderBy(x => x.Order);
                    break;
            }
        }
    }
}
