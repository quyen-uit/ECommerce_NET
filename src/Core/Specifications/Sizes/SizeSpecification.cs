using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Core.Common.Specifications;
using Core.Entities;
using Core.Specifications.Products;

namespace Core.Specifications.Sizes
{
    public class SizeSpecification : BaseSpecification<Size>
    {
        public SizeSpecification(SizeSpecParams specParams)
            : base(x =>
                string.IsNullOrEmpty(specParams.Filter.Name)
                || x.Name.ToLower().Contains(specParams.Filter.Name.ToLower())
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
                case "sort_order_asc":
                    AddOrderBy(x => x.SortOrder);
                    break;
                case "sort_order_desc":
                    AddOrderByDescending(x => x.SortOrder);
                    break;
                default:
                    AddOrderBy(x => x.SortOrder);
                    break;
            }
        }
    }
}
