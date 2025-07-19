using API.Commons.Response;
using API.Helpers;
using AutoMapper;
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
        public AccountController(UserManager<AppUser> userManager, IAccountService accountService, IMapper mapper)
        {
            _userManager = userManager;
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiSuccessResponse<UserDto>>> Login(LoginDto login)
        {
            var user = await _accountService.LoginAsync(login);
            return Ok(ResponseFactory.Ok(user));
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiSuccessResponse<UserDto>>> Register(RegisterDto register)
        {
            var user = await _accountService.RegisterAsync(register);
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
