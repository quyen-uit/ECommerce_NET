using Core.Common.Specifications;
using Core.Entities.Identity;
using Core.Enums;
namespace Core.Specifications.Accounts
{
    public class RolePermissionWithRoleAndPermissionSpecification : BaseSpecification<RolePermission>
    {
        public RolePermissionWithRoleAndPermissionSpecification(List<string> roleNames)
            : base(x =>
                roleNames.Contains(x.Role.Name!))
        {
            AddInclude(x => x.Role);
            AddInclude(x => x.Permission);
        }
    }
}