using Core.Common.Specifications;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Specifications.PriceAdjustments
{
    public class PriceAdjustmentSpecification : BaseSpecification<PriceAdjustment>
    {
        public PriceAdjustmentSpecification(PriceAdjustmentSpecParams specParams, bool isSearch = true)
            : base(x =>
                (string.IsNullOrEmpty(specParams.Filter.Name) || x.Name.ToLower().Contains(specParams.Filter.Name.ToLower()))
                && (!specParams.Filter.StartDate.HasValue || x.StartDate >= specParams.Filter.StartDate.Value)
                && (!specParams.Filter.EndDate.HasValue || x.EndDate <= specParams.Filter.EndDate.Value)
            )
        {
            AddQueryableInclude(x => x.Include(i => i.PriceAdjustmentItems).ThenInclude(i => i.Product));
            if (isSearch)
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
                        AddOrderBy(x => x.UpdatedAt!);
                        break;
                }
            }
        }
    }
}
