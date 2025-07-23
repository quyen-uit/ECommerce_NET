using API.Commons.Response;
using API.Helpers;
using Core.Common;
using Core.Dtos.Accounts;
using Core.Interfaces.Services;
using Core.Specifications.Accounts;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PermissionController : ApiControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiSuccessResponse<PermissionResponse>>> GetPermission(long id)
        {
            var dto = await _permissionService.GetPermissionByIdAsync(id);
            return Ok(ResponseFactory.Ok(dto));
        }

        [HttpPost("get-all")]
        public async Task<ActionResult<ApiSuccessResponse<Pagination<PermissionResponse>>>> GetPermissions(
            [FromBody] PermissionSpecParams specParams
        )
        {
            var result = await _permissionService.GetAllPermissionsAsync(specParams);
            return Ok(ResponseFactory.Ok(result));
        }

        [HttpPost("create")]
        public async Task<ActionResult<ApiSuccessResponse<PermissionResponse>>> CreatePermission([FromBody] CreatePermissionRequest dto)
        {
            var result = await _permissionService.CreatePermissionAsync(dto);
            return Ok(ResponseFactory.Ok(result));
        }

        // [HttpPost("create-many")]
        // public async Task<ActionResult<ApiSuccessResponse<IReadOnlyList<PermissionResponse>>>> CreatePermissions(
        //     [FromBody] IReadOnlyList<CreatePermissionRequest> dtos
        // )
        // {
        //     var result = await _permissionService.AddRangePermissionAsync(dtos);
        //     return Ok(ResponseFactory.Ok(result));
        // }

        [HttpPost("update")]
        public async Task<ActionResult<ApiSuccessResponse<PermissionResponse>>> UpdatePermission([FromBody] UpdatePermissionRequest dto)
        {
            var result = await _permissionService.UpdatePermissionAsync(dto);
            return Ok(ResponseFactory.Ok(result));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiSuccessResponse<object>>> DeletePermission(long id)
        {
            await _permissionService.DeletePermissionAsync(id);
            return Ok(ResponseFactory.Ok());
        }
        // [HttpDelete("delete-many")]
        // public async Task<ActionResult<ApiSuccessResponse<object>>> DeletePermissions([FromBody] List<long> ids)
        // {
        //     await _permissionService.DeletePermissionsAsync(ids);
        //     return Ok(ResponseFactory.Ok());
        // }
    }
}