using Core.Entities.Identity;
using Core.Interfaces.Reposiories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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
            .Include<Permission>(rp => rp.Permission)
            .Select(rp => rp.Permission)
            .ToListAsync();
    }

    public async Task AddPermissionsToRoleAsync(string roleId, List<long> permissionIds)
    {
        var existing = await _context.RolePermissions
            .Where(rp => rp.RoleId == roleId && permissionIds.Contains(rp.PermissionId))
            .ToListAsync();

        var newPermissions = permissionIds
            .Where(pid => !existing.Any(e => e.PermissionId == pid))
            .Select(pid => new RolePermission { RoleId = roleId, PermissionId = pid });

        await _context.RolePermissions.AddRangeAsync(newPermissions);
        await _context.SaveChangesAsync();
    }

    public async Task RemovePermissionsFromRoleAsync(string roleId, List<long> permissionIds)
    {
        var toRemove = await _context.RolePermissions
            .Where(rp => rp.RoleId == roleId && permissionIds.Contains(rp.PermissionId))
            .ToListAsync();

        _context.RolePermissions.RemoveRange(toRemove);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAllPermissionsFromRoleAsync(string roleId)
    {
        var toRemove = await _context.RolePermissions
            .Where(rp => rp.RoleId == roleId)
            .ToListAsync();

        _context.RolePermissions.RemoveRange(toRemove);
        await _context.SaveChangesAsync();
    }
}
