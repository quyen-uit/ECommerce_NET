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
    public class ColorWithParamsSpec : BaseSpecification<Color>
    {
        public ColorWithParamsSpec() { }
        public ColorWithParamsSpec(ColorSpecParams specParams)
            : base(x => (string.IsNullOrEmpty(specParams.Search) || x.Name.ToLower().Contains(specParams.Search)))
        {
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
