using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Core.Common.Specifications;
using Core.Entities;
using Core.Specifications.Products;

namespace Core.Specifications.Colors
{
    public class ColorSpecification : BaseSpecification<Color>
    {
        public ColorSpecification() { }

        public ColorSpecification(ColorSpecParams specParams)
            : base(x =>
                (
                    (string.IsNullOrEmpty(specParams.Filter.Name) || specParams.Filter.Name.ToLower().Contains(x.Name.ToLower()))
                    && (string.IsNullOrEmpty(specParams.Filter.HexCode) || specParams.Filter.HexCode.ToLower().Contains(x.HexCode.ToLower()))
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
