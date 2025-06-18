using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Core.Common.Specifications;
using Core.Entities;
using Core.Specifications.Products;

namespace Core.Specifications.Categories
{
    public class CategorySpecification : BaseSpecification<Category>
    {
        public CategorySpecification() { }

        public CategorySpecification(CategorySpecParams specParams)
            : base(x => (
                string.IsNullOrEmpty(specParams.Filter.Name) || x.Name.ToLower().Contains(specParams.Filter.Name.ToLower()))
                 && (string.IsNullOrEmpty(specParams.Filter.ParentName) || x.Parent!.Name.ToLower().Contains(specParams.Filter.ParentName.ToLower()))
                 && (!specParams.Filter.IsActive.HasValue || x.IsActive == specParams.Filter.IsActive)
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
