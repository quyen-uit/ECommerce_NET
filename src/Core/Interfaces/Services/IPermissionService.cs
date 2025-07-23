using Core.Common;
using Core.Dtos.Accounts;
using Core.Specifications.Accounts;

namespace Core.Interfaces.Services
{
    public interface IPermissionService
    {
        Task<bool> UserHasPermissionAsync(string userId, string permission);
        Task<List<string>> GetUserPermissionsAsync(string userId);
        //Task<bool> RoleHasPermissionAsync(string roleName, string permission);
        Task<PermissionResponse> CreatePermissionAsync(CreatePermissionRequest request);
        Task<PermissionResponse> UpdatePermissionAsync(UpdatePermissionRequest request);
        Task<bool> DeletePermissionAsync(long permissionId);
        Task<PermissionResponse> GetPermissionByIdAsync(long permissionId);
        Task<Pagination<PermissionResponse>> GetAllPermissionsAsync(PermissionSpecParams specParams);
        //Task<List<PermissionResponse>> GetPermissionsByModuleAsync(string module);
        //Task<List<string>> GetAllCategoriesAsync();
    }
}