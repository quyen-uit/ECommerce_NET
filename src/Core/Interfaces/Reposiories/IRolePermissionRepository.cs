using Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Reposiories
{
    public interface IRolePermissionRepository
    {
        Task<List<Permission>> GetPermissionsByRoleIdAsync(string roleId);
        Task AddPermissionsToRoleAsync(string roleId, List<long> permissionIds);
        Task RemovePermissionsFromRoleAsync(string roleId, List<long> permissionIds);
        Task RemoveAllPermissionsFromRoleAsync(string roleId);
    }
}
