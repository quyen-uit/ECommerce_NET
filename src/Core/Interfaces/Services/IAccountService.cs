using Core.Dtos;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Core.Interfaces.Services
{
    public interface IAccountService
    {
        Task<UserDto> LoginAsync(LoginDto request);
        Task<UserDto> RefreshTokenAsync(string token);

        Task<UserDto> RegisterAsync(RegisterDto request);
        Task LogoutAsync(string refreshToken);
        Task LogoutAllAsync(string userId);
        Task<UserDto> GetCurrentUser(ClaimsPrincipal claimsPrincipal);
    }
}