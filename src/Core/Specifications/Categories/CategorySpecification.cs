using Ardalis.Specification;
using Core.Entities;

namespace Core.Specifications.Categories
{
    public class CategorySpecification : Specification<Category>
    {
        public CategorySpecification() { }

        public CategorySpecification(CategorySpecParams specParams, bool isSearch = true)
        {
            Query.Where(x =>
                (string.IsNullOrEmpty(specParams.Filter.Name) || specParams.Filter.Name.ToLower().Contains(x.Name.ToLower()))
                 && (string.IsNullOrEmpty(specParams.Filter.ParentName) || specParams.Filter.ParentName.ToLower().Contains(x.Parent!.Name.ToLower()))
                 && (!specParams.Filter.IsActive.HasValue || x.IsActive == specParams.Filter.IsActive)
            );
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
                    case "order_asc":
                        Query.OrderBy(x => x.Order);
                        break;
                    case "order_desc":
                        Query.OrderByDescending(x => x.Order);
                        break;
                    default:
                        Query.OrderBy(x => x.Order);
                        break;
                }
            }
        }
    }
}
