using API.Commons.Response;
using API.Exceptions;
using API.Extensions;
using API.Helpers;
using MapsterMapper;
using Core.Dtos;
using Core.Entities.Identity;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController : ApiControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly IConfiguration _config;
        public AccountController(UserManager<AppUser> userManager, IAccountService accountService, IMapper mapper, IConfiguration config)
        {
            _userManager = userManager;
            _accountService = accountService;
            _mapper = mapper;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiSuccessResponse<UserDto>>> Login(LoginDto login)
        {
            var user = await _accountService.LoginAsync(login);
            if (!string.IsNullOrEmpty(user.RefreshToken))
            {
                SetRefreshTokenCookie(user.RefreshToken);
                user.RefreshToken = null;
            }
            return Ok(ResponseFactory.Ok(user));
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiSuccessResponse<UserDto>>> Register(RegisterDto register)
        {
            var user = await _accountService.RegisterAsync(register);
            if (!string.IsNullOrEmpty(user.RefreshToken))
            {
                SetRefreshTokenCookie(user.RefreshToken);
                user.RefreshToken = null;
            }
            return Ok(ResponseFactory.Ok(user));
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ApiSuccessResponse<UserDto>>> GetCurrentUser()
        {
            var user = await _accountService.GetCurrentUser(User);
            return Ok(ResponseFactory.Ok(user));
        }

        [HttpGet("email-exist")]
        public async Task<ActionResult<ApiSuccessResponse<bool>>> CheckEmailExist([FromQuery] string email)
        {
            var result = await _userManager.FindByEmailAsync(email) != null;
            return Ok(ResponseFactory.Ok(result));
        }

        [HttpPost("refresh")]
        [Consumes("application/json")]
        public async Task<ActionResult<ApiSuccessResponse<UserDto>>> Refresh()
        {
            var token = Request.Cookies["rt"];
            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedException("Refresh token is missing");
            }

            var user = await _accountService.RefreshTokenAsync(token);
            if (!string.IsNullOrEmpty(user.RefreshToken))
            {
                SetRefreshTokenCookie(user.RefreshToken);
                user.RefreshToken = null;
            }
            return Ok(ResponseFactory.Ok(user));
        }

        [HttpPost("logout")]
        [Consumes("application/json")]
        public async Task<ActionResult<ApiSuccessResponse<string>>> Logout()
        {
            var token = Request.Cookies["rt"];
            if (!string.IsNullOrEmpty(token))
            {
                await _accountService.LogoutAsync(token);
            }
            Response.Cookies.Delete("rt");
            return Ok(ResponseFactory.Ok("Logged out"));
        }

        [Authorize]
        [HttpPost("logout-all")]
        [Consumes("application/json")]
        public async Task<ActionResult<ApiSuccessResponse<string>>> LogoutAll()
        {
            var user = await _userManager.FindByEmailFromClaimsPrinciple(User);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            await _accountService.LogoutAllAsync(user.Id);
            Response.Cookies.Delete("rt");
            return Ok(ResponseFactory.Ok("Logged out from all sessions"));
        }

        [Authorize]
        [HttpGet("sessions")]
        public async Task<ActionResult<ApiSuccessResponse<IReadOnlyList<UserSessionDto>>>> GetSessions()
        {
            var user = await _userManager.FindByEmailFromClaimsPrinciple(User);
            if (user == null) throw new NotFoundException("User not found");
            var sessions = await _accountService.GetSessionsAsync(user.Id);
            return Ok(ResponseFactory.Ok(sessions));
        }

        [Authorize]
        [HttpPost("sessions/{sessionId:guid}/revoke")]
        [Consumes("application/json")]
        public async Task<ActionResult<ApiSuccessResponse<string>>> RevokeSession(Guid sessionId)
        {
            var user = await _userManager.FindByEmailFromClaimsPrinciple(User);
            if (user == null) throw new NotFoundException("User not found");
            await _accountService.RevokeSessionAsync(user.Id, sessionId, "User revoked");
            return Ok(ResponseFactory.Ok("Session revoked"));
        }

        [Authorize]
        [HttpPost("sessions/revoke-others")]
        [Consumes("application/json")]
        public async Task<ActionResult<ApiSuccessResponse<string>>> RevokeOtherSessions()
        {
            var user = await _userManager.FindByEmailFromClaimsPrinciple(User);
            if (user == null) throw new NotFoundException("User not found");
            var currentRt = Request.Cookies["rt"];
            if (string.IsNullOrEmpty(currentRt)) throw new UnauthorizedException("Refresh token is missing");
            var keepSessionId = await _accountService.GetSessionIdByRefreshTokenAsync(currentRt);
            if (keepSessionId == null) throw new UnauthorizedException("Invalid refresh token");
            await _accountService.RevokeOtherSessionsAsync(user.Id, keepSessionId.Value);
            return Ok(ResponseFactory.Ok("Other sessions revoked"));
        }

        private void SetRefreshTokenCookie(string refreshToken)
        {
            var days = int.Parse(_config["Token:RefreshTokenExpirationDays"] ?? "7");
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(days),
                Path = "/api/account/refresh"
            };
            Response.Cookies.Append("rt", refreshToken, cookieOptions);
        }

        //[Authorize]
        //[HttpGet("address")]
        //public async Task<ActionResult<ApiSuccessResponse<AddressDto>>> GetUserAddress()
        //{
        //    var user = await _userManager.FindUserByClamsPrincipleWithAddress(User);
        //    var result = _mapper.Map<AddressDto>(user!.Address);
        //    return Ok(ResponseFactory.Ok(result));
        //}

        //[Authorize]
        //[HttpPut("address")]
        //public async Task<ActionResult<ApiSuccessResponse<AddressDto>>> UpdateUserAddress(AddressDto addressDto)
        //{
        //    var user = await _userManager.FindUserByClamsPrincipleWithAddress(User);
        //    user!.Address = _mapper.Map<Address>(addressDto);
        //    await _userManager.UpdateAsync(user);
        //    var result = _mapper.Map<AddressDto>(user.Address);
        //    return Ok(ResponseFactory.Ok(result));
        //}
    }
}
