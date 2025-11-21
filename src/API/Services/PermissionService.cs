using Core.Common;
using Core.Exceptions;
using Core.Constants;
using Core.Dtos.Accounts;
using Core.Entities.Identity;
using Core.Interfaces.Reposiories;
using Core.Interfaces.Services;
using Core.Specifications.Accounts;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace API.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IRepository<Permission> _permissionRepository;
        private readonly UserManager<AppUser> _userManager;
        // private readonly IPermissionCacheService _permissionCache; // Commented out - Redis disabled

        public PermissionService(
            IRolePermissionRepository rolePermissionRepository,
            IRepository<Permission> permissionRepository,
            UserManager<AppUser> userManager)
            // IPermissionCacheService permissionCache) // Commented out - Redis disabled
        {
            _rolePermissionRepository = rolePermissionRepository;
            _permissionRepository = permissionRepository;
            _userManager = userManager;
            // _permissionCache = permissionCache; // Commented out - Redis disabled
        }

        public async Task<PermissionResponse> CreatePermissionAsync(CreatePermissionRequest request)
        {
            var entity = request.Adapt<Permission>();
            await _permissionRepository.AddAsync(entity);
            return entity.Adapt<PermissionResponse>();
        }

        public async Task<PermissionResponse> UpdatePermissionAsync(UpdatePermissionRequest request)
        {
            var entity = await _permissionRepository.GetByIdAsync(request.Id);
            if (entity == null)
                throw new NotFoundException(CommonMessage.NotFoundPermission);

            request.Adapt(entity);
            await _permissionRepository.UpdateAsync(entity);

            return entity.Adapt<PermissionResponse>();
        }

        public async Task<bool> DeletePermissionAsync(Guid permissionId)
        {
            var existing = await _permissionRepository.GetByIdAsync(permissionId);
            if (existing == null)
                throw new NotFoundException(CommonMessage.NotFoundPermission);

            await _permissionRepository.DeleteAsync(existing);
            return true;
        }

        public async Task<PermissionResponse> GetPermissionByIdAsync(Guid permissionId)
        {
            var entity = await _permissionRepository.GetByIdAsync(permissionId);
            if (entity == null)
                throw new NotFoundException(CommonMessage.NotFoundPermission);
            return entity.Adapt<PermissionResponse>();
        }

        public async Task<Pagination<PermissionResponse>> GetAllPermissionsAsync(PermissionSpecParams specParams)
        {
            var spec = new PermissionSpecification(specParams);
            var entities = await _permissionRepository.ListAsync(spec);

            var specCount = new PermissionSpecification(specParams, false);
            var count = await _permissionRepository.CountAsync(specCount);

            return new Pagination<PermissionResponse>(
                     pageNumber: specParams.PageNumber,
                     pageSize: specParams.PageSize,
                     pageCount: count,
                     data: entities.Adapt<IReadOnlyList<PermissionResponse>>()
                 );
        }

        public async Task<bool> UserHasPermissionAsync(string userId, string permission)
        {
            // Redis caching disabled - always query database
            // var userPermissions = await _permissionCache.GetPermissionsAsync(userId);
            //
            // if (userPermissions is null)
            // {
            var permissionsList = await GetUserPermissionsAsync(userId);
            var userPermissions = permissionsList.ToHashSet();
            //     await _permissionCache.SetPermissionsAsync(userId, userPermissions, TimeSpan.FromMinutes(30));
            // }

            if (userPermissions.Count == 0)
                return false;

            if (userPermissions.Contains(permission))
                return true;

            // Manage implies all: if user has Module.Manage, they can perform any action in that module
            var parts = permission.Split('.', 2);
            if (parts.Length == 2)
            {
                var module = parts[0];
                var managePermission = $"{module}.Manage";
                if (userPermissions.Contains(managePermission))
                    return true;
            }

            return false;
        }

        public async Task<List<string>> GetUserPermissionsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return new List<string>();

            var userRoles = await _userManager.GetRolesAsync(user);
            var rolePermissions = await _rolePermissionRepository.GetPermissionsByRoleNamesAsync(userRoles.ToList());

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
