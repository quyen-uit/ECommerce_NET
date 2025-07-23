using Core.Entities.Identity;
using Core.Specifications.Accounts;

namespace Core.Interfaces.Reposiories
{
    public interface IRolePermissionRepository
    {
        Task<List<Permission>> GetPermissionsByRoleIdAsync(string roleId);
        Task<List<RolePermission>> GetPermissionsByRoleNamesAsync(List<string> roleNames);
        Task AddPermissionsToRoleAsync(string roleId, List<long> permissionIds);
        Task RemovePermissionsFromRoleAsync(string roleId, List<long> permissionIds);
        Task RemoveAllPermissionsFromRoleAsync(string roleId);

        Task<List<AppRole>> GetRolesAsync(RoleParams roleParams);
        Task<int> CountRoles(RoleParams roleParams);
    }
}
