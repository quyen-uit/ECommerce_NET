using Ardalis.Specification;
using Core.Entities;
using Core.Enums;
namespace Core.Specifications.Sizes
{
    public class SizeSpecification : Specification<Size>
    {
        public SizeSpecification(SizeSpecParams specParams, bool isSearch = true)
        {
            Query.Where(x =>
                (string.IsNullOrEmpty(specParams.Filter.Name) || x.Name.ToLower().Contains(specParams.Filter.Name.ToLower()))
                && (!specParams.Filter.SortOrder.From.HasValue || (x.SortOrder >= specParams.Filter.SortOrder.From.Value))
                && (!specParams.Filter.SortOrder.To.HasValue || (x.SortOrder <= specParams.Filter.SortOrder.To.Value))
                && (string.IsNullOrEmpty(specParams.Filter.SizeType) || x.SizeType == Enum.Parse<SizeType>(specParams.Filter.SizeType)));
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
                    case "sort_order_asc":
                        Query.OrderBy(x => x.SortOrder);
                        break;
                    case "sort_order_desc":
                        Query.OrderByDescending(x => x.SortOrder);
                        break;
                    case "size_type_asc":
                        Query.OrderBy(x => x.SizeType);
                        break;
                    case "size_type_desc":
                        Query.OrderByDescending(x => x.SizeType);
                        break;
                    default:
                        Query.OrderBy(x => x.Name);
                        break;
                }
            }
        }
    }
}