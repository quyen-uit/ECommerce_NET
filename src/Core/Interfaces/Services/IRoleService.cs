using Core.Dtos.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IRoleService
    {
        Task<RoleResponse> CreateRoleAsync(CreateRoleRequest request);
        Task<RoleResponse> UpdateRoleAsync(UpdateRoleRequest request);
        Task<bool> DeleteRoleAsync(string roleId);
        Task<RoleResponse> GetRoleByIdAsync(string roleId);
        Task<RoleResponse> GetRoleByNameAsync(string roleName);
        Task<List<RoleResponse>> GetAllRolesAsync();
        Task<bool> AssignPermissionToRoleAsync(string roleId, int permissionId);
        Task<bool> RemovePermissionFromRoleAsync(string roleId, int permissionId);
        Task<List<PermissionResponse>> GetRolePermissionsAsync(string roleId);
        Task<bool> RoleExistsAsync(string roleName);
    }
}
