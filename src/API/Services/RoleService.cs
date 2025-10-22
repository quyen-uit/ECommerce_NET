using Core.Common;
using Core.Constants;
using Core.Dtos.Accounts;
using Core.Entities.Identity;
using Core.Interfaces;
using Core.Interfaces.Services;
using Core.Specifications.Accounts;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace API.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;


        public RoleService(
            RoleManager<AppRole> roleManager,
            UserManager<AppUser> userManager,
            IUnitOfWork unitOfWork)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<RoleResponse> CreateRoleAsync(CreateRoleRequest request)
        {
            var role = request.Adapt<AppRole>();

            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Failed to create role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
            await _unitOfWork.RolePermissionRepository.AddPermissionsToRoleAsync(role.Id, request.PermissionIds);
            await _unitOfWork.Complete();

            return await GetRoleByIdAsync(role.Id);
        }

        public async Task<RoleResponse> UpdateRoleAsync(UpdateRoleRequest request)
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
            await _unitOfWork.Complete();

            return await GetRoleByIdAsync(role.Id);
        }

        public async Task<bool> DeleteRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                throw new NotFoundException(CommonMessage.NotFoundRole);

            // Check if any users are assigned to this role
            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);
            if (usersInRole.Any())
            {
                throw new InvalidOperationException("Cannot delete role that has assigned users");
            }

            // Remove role permissions
            await _unitOfWork.RolePermissionRepository.RemoveAllPermissionsFromRoleAsync(roleId);
            await _unitOfWork.Complete();

            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded;
        }

        public async Task<RoleResponse> GetRoleByIdAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                throw new NotFoundException(CommonMessage.NotFoundRole);
            var permissions = await _unitOfWork.RolePermissionRepository.GetPermissionsByRoleIdAsync(roleId);
            var users = await _userManager.GetUsersInRoleAsync(role.Name!);

            var roleResponse = role.Adapt<RoleResponse>();
            roleResponse.UserCount = users.Count;
            foreach (var permission in permissions)
                roleResponse.Permissions.Add(permission.Adapt<PermissionResponse>());

            return roleResponse;
        }

        public async Task<Pagination<RoleResponse>> GetAllRolesAsync(RoleParams roleParams)
        {
            var roles = await _unitOfWork.RolePermissionRepository.GetRolesAsync(roleParams);
            var countAllRoles = await _unitOfWork.RolePermissionRepository.CountRoles(roleParams);

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

        private async Task UpdateRolePermissionsAsync(string roleId, List<long> permissionIds)
        {
            // get all rolepermission
            var permissions = await _unitOfWork.RolePermissionRepository.GetPermissionsByRoleIdAsync(roleId);
            var existIds = permissions.Select(p => p.Id).ToList();
            var deletePermissionIds = existIds.Where(p => !permissionIds.Contains(p)).ToList();

            // remove permission not exist in permissionIds
            if (deletePermissionIds != null && deletePermissionIds.Count > 0)
                await _unitOfWork.RolePermissionRepository.RemovePermissionsFromRoleAsync(roleId, deletePermissionIds);

            // add new permssionIds
            var addPermissionIds = permissionIds.Where(p => !existIds.Contains(p)).ToList();
            await _unitOfWork.RolePermissionRepository.AddPermissionsToRoleAsync(roleId, addPermissionIds);

        }
    }
}