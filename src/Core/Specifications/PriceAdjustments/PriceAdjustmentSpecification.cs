using Ardalis.Specification;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Specifications.PriceAdjustments
{
    public class PriceAdjustmentSpecification : Specification<PriceAdjustment>
    {
        public PriceAdjustmentSpecification(PriceAdjustmentSpecParams specParams, bool isSearch = true)
        {
            Query.Where(x =>
                (string.IsNullOrEmpty(specParams.Filter.Name) || x.Name.ToLower().Contains(specParams.Filter.Name.ToLower()))
                && (!specParams.Filter.StartDate.HasValue || x.StartDate >= specParams.Filter.StartDate.Value)
                && (!specParams.Filter.EndDate.HasValue || x.EndDate <= specParams.Filter.EndDate.Value)
            );
            Query.Include(i => i.PriceAdjustmentItems).ThenInclude(i => i.Product);
            if (isSearch)
            {
                Query.Skip(specParams.PageSize * (specParams.PageNumber - 1))
                     .Take(specParams.PageSize);
                switch (specParams.Sort)
                {
                    case "name_asc":
                        Query.OrderBy(x => x.Name);
                        break;
                    case "name_desc":
                        Query.OrderByDescending(x => x.Name);
                        break;
                    case "start_date_asc":
                        Query.OrderBy(x => x.StartDate);
                        break;
                    case "start_date_desc":
                        Query.OrderByDescending(x => x.StartDate);
                        break;
                    case "end_date_asc":
                        Query.OrderBy(x => x.EndDate);
                        break;
                    case "end_date_desc":
                        Query.OrderByDescending(x => x.EndDate);
                        break;
                    default:
                        Query.OrderBy(x => x.UpdatedAt!);
                        break;
                }
            }
        }
    }
}
