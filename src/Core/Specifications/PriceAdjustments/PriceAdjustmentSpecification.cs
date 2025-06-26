using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Core.Common.Specifications;
using Core.Entities;
using Core.Specifications.Products;
using Microsoft.EntityFrameworkCore;

namespace Core.Specifications.PriceAdjustments
{
    public class PriceAdjustmentSpecification : BaseSpecification<PriceAdjustment>
    {
        public PriceAdjustmentSpecification(PriceAdjustmentSpecParams specParams)
            : base(x =>
                (string.IsNullOrEmpty(specParams.Filter.Name) || specParams.Filter.Name.ToLower().Contains(x.Name.ToLower()))
                && (!specParams.Filter.StartDate.HasValue || (x.StartDate >= specParams.Filter.StartDate.Value && x.StartDate <= specParams.Filter.StartDate.Value))
                && (!specParams.Filter.EndDate.HasValue || (x.EndDate >= specParams.Filter.EndDate.Value && x.EndDate <= specParams.Filter.EndDate.Value))
            )
        {
            AddQueryableInclude(x => x.Include(i => i.PriceAdjustmentItems).ThenInclude(i => i.Product));
            AddPagination(specParams.PageSize, specParams.PageNumber);
            switch (specParams.Sort)
            {
                case "name_asc":
                    AddOrderBy(x => x.Name);
                    break;
                case "name_desc":
                    AddOrderByDescending(x => x.Name);
                    break;
                case "start_date_asc":
                    AddOrderBy(x => x.StartDate);
                    break;
                case "start_date_desc":
                    AddOrderByDescending(x => x.StartDate);
                    break;
                case "end_date_asc":
                    AddOrderBy(x => x.EndDate);
                    break;
                case "end_date_desc":
                    AddOrderByDescending(x => x.EndDate);
                    break;
                default:
                    AddOrderBy(x => x.UpdatedDatetime!);
                    break;
            }
        }
    }
}
