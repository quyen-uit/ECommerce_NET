using Core.Common;
using Core.Constants;
using Core.Dtos.Accounts;
using Core.Entities.Identity;
using Core.Interfaces.Services;
using Core.Interfaces.Reposiories;
using Core.Interfaces.Services;
using Core.Specifications.Accounts;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace API.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly ITransactionCoordinator _tx;
        private readonly IMemoryCache _cache;


        public RoleService(
            RoleManager<AppRole> roleManager,
            UserManager<AppUser> userManager,
            IRolePermissionRepository rolePermissionRepository,
            ITransactionCoordinator tx,
            IMemoryCache cache)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _rolePermissionRepository = rolePermissionRepository;
            _tx = tx;
            _cache = cache;
        }

        public async Task<RoleResponse> CreateRoleAsync(CreateRoleRequest request)
        {
            return await _tx.ExecuteAsync(async ct =>
            {
                var role = request.Adapt<AppRole>();
                var result = await _roleManager.CreateAsync(role);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException($"Failed to create role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
                await _rolePermissionRepository.AddPermissionsToRoleAsync(role.Id, request.PermissionIds);
                return await GetRoleByIdAsync(role.Id);
            });
        }

        public async Task<RoleResponse> UpdateRoleAsync(UpdateRoleRequest request)
        {
            return await _tx.ExecuteAsync(async ct =>
            {
                var role = await _roleManager.FindByIdAsync(request.Id);
                if (role == null)
                    throw new NotFoundException(CommonMessage.NotFoundRole);

                role = request.Adapt<AppRole>();

                var result = await _roleManager.UpdateAsync(role);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException($"Failed to update role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }

                // Update role permissions
                await UpdateRolePermissionsAsync(role.Id, request.PermissionIds);

                // Invalidate permission cache for users in this role
                await InvalidateUsersPermissionCacheAsync(role.Name!);

                return await GetRoleByIdAsync(role.Id);
            });
        }

        public async Task<bool> DeleteRoleAsync(string roleId)
        {
            return await _tx.ExecuteAsync(async ct =>
            {
                var role = await _roleManager.FindByIdAsync(roleId);
                if (role == null)
                    throw new NotFoundException(CommonMessage.NotFoundRole);

                var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);
                if (usersInRole.Any())
                {
                    throw new InvalidOperationException("Cannot delete role that has assigned users");
                }

                await _rolePermissionRepository.RemoveAllPermissionsFromRoleAsync(roleId);
                var result = await _roleManager.DeleteAsync(role);
                return result.Succeeded;
            });
        }

        public async Task<RoleResponse> GetRoleByIdAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                throw new NotFoundException(CommonMessage.NotFoundRole);
            var permissions = await _rolePermissionRepository.GetPermissionsByRoleIdAsync(roleId);
            var users = await _userManager.GetUsersInRoleAsync(role.Name!);

            var roleResponse = role.Adapt<RoleResponse>();
            roleResponse.UserCount = users.Count;
            foreach (var permission in permissions)
                roleResponse.Permissions.Add(permission.Adapt<PermissionResponse>());

            return roleResponse;
        }

        public async Task<Pagination<RoleResponse>> GetAllRolesAsync(RoleParams roleParams)
        {
            var roles = await _rolePermissionRepository.GetRolesAsync(roleParams);
            var countAllRoles = await _rolePermissionRepository.CountRoles(roleParams);

            var roleResponses = roles.Adapt<List<RoleResponse>>();
            foreach (var role in roleResponses)
            {
                var users = await _userManager.GetUsersInRoleAsync(role.Name);
                role.UserCount = users.Count;

            }

            return new Pagination<RoleResponse>(
                    pageNumber: roleParams.PageNumber,
                    pageSize: roleParams.PageSize,
                    pageCount: countAllRoles,
                    data: roleResponses
                );
        }

        private async Task UpdateRolePermissionsAsync(string roleId, List<Guid> permissionIds)
        {
            // get all rolepermission
            var permissions = await _rolePermissionRepository.GetPermissionsByRoleIdAsync(roleId);
            var existIds = permissions.Select(p => p.Id).ToList();
            var deletePermissionIds = existIds.Where(p => !permissionIds.Contains(p)).ToList();

            // remove permission not exist in permissionIds
            if (deletePermissionIds != null && deletePermissionIds.Count > 0)
            await _rolePermissionRepository.RemovePermissionsFromRoleAsync(roleId, deletePermissionIds);

            // add new permssionIds
            var addPermissionIds = permissionIds.Where(p => !existIds.Contains(p)).ToList();
            await _rolePermissionRepository.AddPermissionsToRoleAsync(roleId, addPermissionIds);

        }

        private async Task InvalidateUsersPermissionCacheAsync(string roleName)
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
            foreach (var user in usersInRole)
            {
                var cacheKey = $"user_permissions_{user.Id}";
                _cache.Remove(cacheKey);
            }
        }
    }
}
