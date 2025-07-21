using Core.Dtos.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IPermissionService
    {
        Task<bool> UserHasPermissionAsync(string userId, string permission);
        Task<List<string>> GetUserPermissionsAsync(string userId);
        //Task<bool> RoleHasPermissionAsync(string roleName, string permission);
        Task<PermissionResponse> CreateOrUpdatePermissionAsync(CreatePermissionRequest request);
        Task<bool> DeletePermissionAsync(int permissionId);
        Task<PermissionResponse> GetPermissionByIdAsync(int permissionId);
        Task<PermissionResponse> GetPermissionByNameAsync(string permissionName);
        Task<List<PermissionResponse>> GetAllPermissionsAsync();
        //Task<List<PermissionResponse>> GetPermissionsByModuleAsync(string module);
        //Task<List<string>> GetAllCategoriesAsync();
        Task<bool> PermissionExistsAsync(string permissionName);
    }
}
