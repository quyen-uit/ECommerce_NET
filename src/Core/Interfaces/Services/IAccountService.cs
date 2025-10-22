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
        Task<IReadOnlyList<UserSessionDto>> GetSessionsAsync(string userId);
        Task RevokeSessionAsync(string userId, Guid sessionId, string? reason = null);
        Task RevokeOtherSessionsAsync(string userId, Guid keepSessionId);
        Task<Guid?> GetSessionIdByRefreshTokenAsync(string refreshToken);
    }
}
