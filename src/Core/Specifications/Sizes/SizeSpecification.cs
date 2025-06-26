using Core.Common.Specifications;
using Core.Entities;
namespace Core.Specifications.Sizes
{
    public class SizeSpecification : BaseSpecification<Size>
    {
        public SizeSpecification(SizeSpecParams specParams)
            : base(x =>
                (string.IsNullOrEmpty(specParams.Filter.Name) || x.Name.ToLower().Contains(specParams.Filter.Name.ToLower()))
                && (!specParams.Filter.SortOrder.From.HasValue || (x.SortOrder >= specParams.Filter.SortOrder.From.Value))
                && (!specParams.Filter.SortOrder.To.HasValue || (x.SortOrder <= specParams.Filter.SortOrder.To.Value))
                && (string.IsNullOrEmpty(specParams.Filter.SizeStype) || x.SizeType.ToString().ToLower().Contains(specParams.Filter.SizeStype.ToLower()))
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
                case "size_type_asc":
                    AddOrderBy(x => x.SizeType);
                    break;
                case "size_type_desc":
                    AddOrderByDescending(x => x.SizeType);
                    break;
                default:
                    AddOrderBy(x => x.SortOrder);
                    break;
            }
        }
    }
}