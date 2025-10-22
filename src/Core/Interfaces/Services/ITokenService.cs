using Core.Entities.Identity;

namespace Core.Interfaces.Services
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
        Task<string> CreateRefreshToken(string userId, Guid? sessionId = null, string? createdByIp = null, string? userAgent = null, string? deviceName = null);
    }
}
