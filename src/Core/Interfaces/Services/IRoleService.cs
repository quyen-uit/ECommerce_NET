using Core.Common;
using Core.Dtos.Accounts;
using Core.Specifications.Accounts;

namespace Core.Interfaces.Services
{
    public interface IRoleService
    {
        Task<RoleResponse> CreateRoleAsync(CreateRoleRequest request);
        Task<RoleResponse> UpdateRoleAsync(UpdateRoleRequest request);
        Task<bool> DeleteRoleAsync(string roleId);
        Task<RoleResponse> GetRoleByIdAsync(string roleId);
        Task<Pagination<RoleResponse>> GetAllRolesAsync(RoleParams roleParams);
    }
}
