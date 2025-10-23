using Core.Entities.Identity;
using Core.Interfaces.Reposiories;
using Core.Specifications.Accounts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class RolePermissionRepository : IRolePermissionRepository
    {
        private readonly ApplicationDbContext _context;

        public RolePermissionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Permission>> GetPermissionsByRoleIdAsync(string roleId)
        {
            return await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Select(rp => rp.Permission)
                .ToListAsync();
        }

        public async Task AddPermissionsToRoleAsync(string roleId, List<Guid> permissionIds)
        {
            var newRolePermissions = permissionIds.Select(pid => new RolePermission { RoleId = roleId, PermissionId = pid });
            await _context.RolePermissions.AddRangeAsync(newRolePermissions);
        }

        public async Task RemovePermissionsFromRoleAsync(string roleId, List<Guid> permissionIds)
        {
            var toRemove = await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId && permissionIds.Contains(rp.PermissionId))
                .ToListAsync();
            _context.RolePermissions.RemoveRange(toRemove);
        }

        public async Task RemoveAllPermissionsFromRoleAsync(string roleId)
        {
            var toRemove = await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .ToListAsync();
            _context.RolePermissions.RemoveRange(toRemove);
        }

        public async Task<List<RolePermission>> GetPermissionsByRoleNamesAsync(List<string> roleNames)
        {
            var rolePermissions = await _context.RolePermissions
            .Where(rp => roleNames.Contains(rp.Role.Name!))
            .Include(rp => rp.Role)
            .Include(rp => rp.Permission)
            .ToListAsync();
            return rolePermissions ?? new List<RolePermission>();
        }

        public async Task<List<AppRole>> GetRolesAsync(RoleParams roleParams)
        {
            var roles = await _context.Roles.Where(rp =>
                (string.IsNullOrEmpty(roleParams.Filter.Name) || rp.Name!.ToLower().Contains(roleParams.Filter.Name.ToLower()))
                && (string.IsNullOrEmpty(roleParams.Filter.Description) || rp.Description.ToLower().Contains(roleParams.Filter.Description.ToLower())))
                .Skip(roleParams.PageSize * (roleParams.PageNumber - 1))
                .Take(roleParams.PageSize)
                .ToListAsync();
            return roles;
        }

        public async Task<int> CountRoles(RoleParams roleParams)
        {
            var count = await _context.Roles.Where(rp =>
                (string.IsNullOrEmpty(roleParams.Filter.Name) || rp.Name!.ToLower().Contains(roleParams.Filter.Name.ToLower()))
                && (string.IsNullOrEmpty(roleParams.Filter.Description) || rp.Description.ToLower().Contains(roleParams.Filter.Description.ToLower())))
                .CountAsync();
            return count;
        }

    }
}
