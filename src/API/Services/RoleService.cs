using Core.Common;
using Core.Constants;
using Core.Dtos.Accounts;
using Core.Entities.Identity;
using Core.Exceptions;
using Core.Interfaces.Services;
using Core.Interfaces.Reposiories;
using Core.Specifications.Accounts;
using Infrastructure.Data;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        // private readonly IPermissionCacheService _permissionCache; // Commented out - Redis disabled
        private readonly ApplicationDbContext _context;


        public RoleService(
            RoleManager<AppRole> roleManager,
            UserManager<AppUser> userManager,
            IRolePermissionRepository rolePermissionRepository,
            // IPermissionCacheService permissionCache, // Commented out - Redis disabled
            ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _rolePermissionRepository = rolePermissionRepository;
            // _permissionCache = permissionCache; // Commented out - Redis disabled
            _context = context;
        }

        public async Task<RoleResponse> CreateRoleAsync(CreateRoleRequest request)
        {
            // Using DbContext.Database.BeginTransaction for explicit transaction
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var role = request.Adapt<AppRole>();
                var result = await _roleManager.CreateAsync(role);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException($"Failed to create role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
                await _rolePermissionRepository.AddPermissionsToRoleAsync(role.Id, request.PermissionIds);
                await transaction.CommitAsync();
                return await GetRoleByIdAsync(role.Id);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<RoleResponse> UpdateRoleAsync(UpdateRoleRequest request)
        {
            // Using DbContext.Database.BeginTransaction for explicit transaction
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
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

                // Invalidate permission cache for users in this role - Commented out - Redis disabled
                // await InvalidateUsersPermissionCacheAsync(role.Name!);

                await transaction.CommitAsync();
                return await GetRoleByIdAsync(role.Id);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> DeleteRoleAsync(string roleId)
        {
            // Using DbContext.Database.BeginTransaction for explicit transaction
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
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
                await transaction.CommitAsync();
                return result.Succeeded;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
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

            // Fix: Get user counts in a single query instead of N queries
            var roleIds = roleResponses.Select(r => r.Id).ToList();
            var userCounts = await _context.UserRoles
                .Where(ur => roleIds.Contains(ur.RoleId))
                .GroupBy(ur => ur.RoleId)
                .Select(g => new { RoleId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.RoleId, x => x.Count);

            foreach (var role in roleResponses)
            {
                role.UserCount = userCounts.GetValueOrDefault(role.Id, 0);
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

        // Commented out - Redis disabled
        // private async Task InvalidateUsersPermissionCacheAsync(string roleName)
        // {
        //     var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
        //     foreach (var user in usersInRole)
        //     {
        //         await _permissionCache.InvalidatePermissionsAsync(user.Id);
        //     }
        // }
    }
}
