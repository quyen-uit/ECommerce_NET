using Core.Dtos.Accounts;
using Core.Entities.Identity;
using Core.Interfaces.Reposiories;
using Core.Interfaces.Services;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IGenericRepository<Permission> _permissionRepository;
        private readonly IGenericRepository<RolePermission> _rolePermissionRepository;

        public RoleManagementService(
            RoleManager<AppRole> roleManager,
            UserManager<AppUser> userManager,
            IGenericRepository<Permission> permissionRepository,
            IGenericRepository<RolePermission> rolePermissionRepository)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _permissionRepository = permissionRepository;
            _rolePermissionRepository = rolePermissionRepository;
        }

        public async Task<RoleResponse> CreateRoleAsync(CreateRoleRequest request)
        {
            var role = request.Adapt<AppRole>();

            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Failed to create role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            return await GetRoleByIdAsync(role.Id);
        }

        public async Task<RoleResponse> UpdateRoleAsync(UpdateRoleRequest request)
        {
            var role = await _roleManager.FindByIdAsync(request.Id);
            if (role == null)
                throw new NotFoundException("Role not found");

            role = request.Adapt<AppRole>();

            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Failed to update role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            // Update role permissions
            await UpdateRolePermissionsAsync(role.Id, request.PermissionIds);

            return await GetRoleByIdAsync(role.Id);
        }

        public async Task<bool> DeleteRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return false;

            // Check if any users are assigned to this role
            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
            if (usersInRole.Any())
            {
                throw new InvalidOperationException("Cannot delete role that has assigned users");
            }

            // Remove role permissions
            var rolePermissions = _context.RolePermissions.Where(rp => rp.RoleId == roleId);
            _context.RolePermissions.RemoveRange(rolePermissions);
            await _context.SaveChangesAsync();

            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded;
        }

        public async Task<RoleResponse> GetRoleByIdAsync(string roleId)
        {
            var role = await _context.Roles
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(r => r.Id == roleId);

            if (role == null)
                return null;

            var userCount = await _userManager.GetUsersInRoleAsync(role.Name);

            return new RoleResponse
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                UserCount = userCount.Count,
                Permissions = role.RolePermissions.Select(rp => new PermissionResponse
                {
                    Id = rp.Permission.Id,
                    Name = rp.Permission.Name,
                    Description = rp.Permission.Description,
                    Category = rp.Permission.Category,
                    Action = rp.Permission.Action
                }).ToList()
            };
        }

        public async Task<RoleResponse> GetRoleByNameAsync(string roleName)
        {
            var role = await _context.Roles
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(r => r.Name == roleName);

            if (role == null)
                return null;

            var userCount = await _userManager.GetUsersInRoleAsync(role.Name);

            return new RoleResponse
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                UserCount = userCount.Count,
                Permissions = role.RolePermissions.Select(rp => new PermissionResponse
                {
                    Id = rp.Permission.Id,
                    Name = rp.Permission.Name,
                    Description = rp.Permission.Description,
                    Category = rp.Permission.Category,
                    Action = rp.Permission.Action
                }).ToList()
            };
        }

        public async Task<List<RoleResponse>> GetAllRolesAsync()
        {
            var roles = await _context.Roles
                .Include(r => r.RolePermissions)
                .ThenInclude(rp => rp.Permission)
                .ToListAsync();

            var roleResponses = new List<RoleResponse>();

            foreach (var role in roles)
            {
                var userCount = await _userManager.GetUsersInRoleAsync(role.Name);

                roleResponses.Add(new RoleResponse
                {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description,
                    UserCount = userCount.Count,
                    Permissions = role.RolePermissions.Select(rp => new PermissionResponse
                    {
                        Id = rp.Permission.Id,
                        Name = rp.Permission.Name,
                        Description = rp.Permission.Description,
                        Category = rp.Permission.Category,
                        Action = rp.Permission.Action
                    }).ToList()
                });
            }

            return roleResponses;
        }

        public async Task<bool> AssignPermissionToRoleAsync(string roleId, int permissionId)
        {
            var rolePermission = await _context.RolePermissions
                .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

            if (rolePermission != null)
                return true; // Already assigned

            _context.RolePermissions.Add(new RolePermission
            {
                RoleId = roleId,
                PermissionId = permissionId
            });

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemovePermissionFromRoleAsync(string roleId, int permissionId)
        {
            var rolePermission = await _context.RolePermissions
                .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

            if (rolePermission == null)
                return false;

            _context.RolePermissions.Remove(rolePermission);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<PermissionResponse>> GetRolePermissionsAsync(string roleId)
        {
            var permissions = await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Include(rp => rp.Permission)
                .Select(rp => new PermissionResponse
                {
                    Id = rp.Permission.Id,
                    Name = rp.Permission.Name,
                    Description = rp.Permission.Description,
                    Category = rp.Permission.Category,
                    Action = rp.Permission.Action
                })
                .ToListAsync();

            return permissions;
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        private async Task AssignPermissionsToRoleAsync(string roleId, List<int> permissionIds)
        {
            var rolePermissions = permissionIds.Select(permissionId => new RolePermission
            {
                RoleId = roleId,
                PermissionId = permissionId
            }).ToList();

            _rolePermissionRepository.AddRange(rolePermissions);
            await _rolePermissionRepository.Complete();
        }

        private async Task UpdateRolePermissionsAsync(string roleId, List<int> permissionIds)
        {
            // Remove existing permissions
            var existingPermissions = _context.RolePermissions.Where(rp => rp.RoleId == roleId);
            _context.RolePermissions.RemoveRange(existingPermissions);

            // Add new permissions
            if (permissionIds?.Any() == true)
            {
                await AssignPermissionsToRoleAsync(roleId, permissionIds);
            }
        }
    }
}