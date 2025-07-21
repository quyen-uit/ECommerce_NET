using API.Helpers;
using Core.Entities.Identity;
using Core.Interfaces.Reposiories;
using Core.Interfaces.Services;
using Core.Specifications.Accounts;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace API.Services
{
    public class PermissionService : IPermissionService
    {
        //private readonly IGenericRepository<Permission> _permissionRepo;
        private readonly IGenericRepository<RolePermission> _rolePermissionRepo;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMemoryCache _cache;

        public PermissionService(
            IGenericRepository<Permission> permissionRepo,
            IGenericRepository<RolePermission> rolePermissionRepo,
            UserManager<IdentityUser> userManager,
            IMemoryCache cache)
        {
            //_permissionRepo = permissionRepo;
            _rolePermissionRepo = rolePermissionRepo;
            _userManager = userManager;
            _cache = cache;
        }

        public async Task<bool> UserHasPermissionAsync(string userId, string permission)
        {
            var cacheKey = $"user_permissions_{userId}";

            if (!_cache.TryGetValue(cacheKey, out List<string>? userPermissions))
            {
                userPermissions = await GetUserPermissionsAsync(userId);
                _cache.Set(cacheKey, userPermissions, TimeSpan.FromMinutes(30));
            }

            return userPermissions!.Contains(permission);
        }

        public async Task<List<string>> GetUserPermissionsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return new List<string>();

            var userRoles = await _userManager.GetRolesAsync(user);

            var spec = new RolePermissionWithRoleAndPermissionSpecification(userRoles.ToList());
            var rolePermissions = await _rolePermissionRepo.GetAllWithSpecAsync(spec);

            return rolePermissions
                .Select(rp => rp.Permission.Name)
                .Distinct()
                .ToList();
        }

        // public async Task<bool> RoleHasPermissionAsync(string roleName, string permission)
        // {
        //     var rolePermission = await _rolePermissionRepo.GetFirstOrDefaultAsync(
        //         rp => rp.Role.Name == roleName && rp.Permission.Name == permission,
        //         include: query => query.Include(rp => rp.Role).Include(rp => rp.Permission));

        //     return rolePermission != null;
        // }
    }
}
