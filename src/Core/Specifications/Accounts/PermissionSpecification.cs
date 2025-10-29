using Ardalis.Specification;
using Core.Entities.Identity;
using Core.Enums;
namespace Core.Specifications.Accounts
{
    public class PermissionSpecification : Specification<Permission>
    {
        public PermissionSpecification(PermissionSpecParams specParams, bool isSearch = true)
        {
            Query.Where(x =>
                (string.IsNullOrEmpty(specParams.Filter.Name) || x.Name.ToLower().Contains(specParams.Filter.Name.ToLower()))
                && (string.IsNullOrEmpty(specParams.Filter.Description) || x.Name.ToLower().Contains(specParams.Filter.Description.ToLower()))
                && (string.IsNullOrEmpty(specParams.Filter.Action) || x.Name.ToLower().Contains(specParams.Filter.Action.ToLower()))
                && (string.IsNullOrEmpty(specParams.Filter.Module) || x.Module == Enum.Parse<AppModule>(specParams.Filter.Module)));
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
                    case "description_asc":
                        Query.OrderBy(x => x.Description);
                        break;
                    case "description_desc":
                        Query.OrderByDescending(x => x.Description);
                        break;
                    case "module_asc":
                        Query.OrderBy(x => x.Module);
                        break;
                    case "module_desc":
                        Query.OrderByDescending(x => x.Module);
                        break;
                    case "action_asc":
                        Query.OrderBy(x => x.Action);
                        break;
                    case "action_desc":
                        Query.OrderByDescending(x => x.Action);
                        break;
                    default:
                        Query.OrderBy(x => x.Name);
                        break;
                }
            }
        }
    }
}