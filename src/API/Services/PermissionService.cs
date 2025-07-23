using Core.Common;
using Core.Constants;
using Core.Dtos.Accounts;
using Core.Entities.Identity;
using Core.Interfaces;
using Core.Interfaces.Services;
using Core.Specifications.Accounts;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace API.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMemoryCache _cache;

        public PermissionService(
            IUnitOfWork unitOfWork,
            UserManager<AppUser> userManager,
            IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _cache = cache;
        }

        public async Task<PermissionResponse> CreatePermissionAsync(CreatePermissionRequest request)
        {
            var entity = request.Adapt<Permission>();
            _unitOfWork.Repository<Permission>().Add(entity);

            await _unitOfWork.Complete();
            return entity.Adapt<PermissionResponse>();
        }

        public async Task<PermissionResponse> UpdatePermissionAsync(UpdatePermissionRequest request)
        {
            var entity = await _unitOfWork.Repository<Permission>().GetByIdAsync(request.Id);
            if (entity == null)
                throw new NotFoundException(CommonMessage.NotFoundPermission);

            request.Adapt(entity);
            _unitOfWork.Repository<Permission>().Update(entity);
            await _unitOfWork.Complete();

            return entity.Adapt<PermissionResponse>();
        }

        public async Task<bool> DeletePermissionAsync(long permissionId)
        {
            var existing = await _unitOfWork.Repository<Permission>().GetByIdAsync(permissionId);
            if (existing == null)
                throw new NotFoundException(CommonMessage.NotFoundPermission);

            _unitOfWork.Repository<Permission>().Delete(existing);
            await _unitOfWork.Complete();
            return true;
        }

        public async Task<PermissionResponse> GetPermissionByIdAsync(long permissionId)
        {
            var entity = await _unitOfWork.Repository<Permission>().GetByIdAsync(permissionId);
            if (entity == null)
                throw new NotFoundException(CommonMessage.NotFoundPermission);
            return entity.Adapt<PermissionResponse>();
        }

        public async Task<Pagination<PermissionResponse>> GetAllPermissionsAsync(PermissionSpecParams specParams)
        {
            var spec = new PermissionSpecification(specParams);
            var entities = await _unitOfWork.Repository<Permission>().GetAllWithSpecAsync(spec);

            var specCount = new PermissionSpecification(specParams, false);
            var count = await _unitOfWork.Repository<Permission>().CountAsync(specCount);

            return new Pagination<PermissionResponse>(
                     pageNumber: specParams.PageNumber,
                     pageSize: specParams.PageSize,
                     pageCount: count,
                     data: entities.Adapt<IReadOnlyList<PermissionResponse>>()
                 );
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
            var rolePermissions = await _unitOfWork.RolePermissionRepository.GetPermissionsByRoleNamesAsync(userRoles.ToList());

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
