using Core.Common.Specifications;
using Core.Entities.Identity;
using Core.Enums;
namespace Core.Specifications.Accounts
{
    public class PermissionSpecification : BaseSpecification<Permission>
    {
        public PermissionSpecification(PermissionSpecParams specParams, bool isSearch = true)
            : base(x =>
                (string.IsNullOrEmpty(specParams.Filter.Name) || x.Name.ToLower().Contains(specParams.Filter.Name.ToLower()))
                && (string.IsNullOrEmpty(specParams.Filter.Description) || x.Name.ToLower().Contains(specParams.Filter.Description.ToLower()))
                && (string.IsNullOrEmpty(specParams.Filter.Action) || x.Name.ToLower().Contains(specParams.Filter.Action.ToLower()))
                && (string.IsNullOrEmpty(specParams.Filter.Module) || x.Module == Enum.Parse<AppModule>(specParams.Filter.Module)))
        {
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
                    case "description_asc":
                        AddOrderBy(x => x.Description);
                        break;
                    case "description_desc":
                        AddOrderByDescending(x => x.Description);
                        break;
                    case "module_asc":
                        AddOrderBy(x => x.Module);
                        break;
                    case "module_desc":
                        AddOrderByDescending(x => x.Module);
                        break;
                    case "action_asc":
                        AddOrderBy(x => x.Action);
                        break;
                    case "action_desc":
                        AddOrderByDescending(x => x.Action);
                        break;
                    default:
                        AddOrderBy(x => x.Name);
                        break;
                }
            }
        }
    }
}