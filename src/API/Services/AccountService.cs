using Core.Exceptions;
using API.Extensions;
using API.Options;
using Core.Dtos;
using Core.Entities.Identity;
using Core.Interfaces;
using Core.Interfaces.Services;
using Core.Interfaces.Reposiories;
using Core.Specifications.Accounts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace API.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JwtTokenOptions _tokenOptions;

        public AccountService(
            UserManager<AppUser> userManager,
            ITokenService tokenService,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            IOptions<JwtTokenOptions> tokenOptions)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _tokenOptions = tokenOptions.Value;
        }

        public async Task<UserDto> LoginAsync(LoginDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                throw new UnauthorizedException("Invalid email or password");
            }

            var accessToken = await _tokenService.CreateToken(user);
            var (ip, ua, device) = GetClientInfo();
            var newSessionId = Guid.NewGuid();
            var refreshToken = await _tokenService.CreateRefreshToken(user.Id, newSessionId, ip, ua, device);

            await EnforceMaxSessionsAsync(user.Id, ip);

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email!,
                Token = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<UserDto> RefreshTokenAsync(string token)
        {
            var refreshTokenRepo = _unitOfWork.Repository<RefreshToken>();
            var storedToken = await refreshTokenRepo.FirstOrDefaultAsync(new RefreshTokenWithUserSpecification(token));
            if (storedToken == null)
            {
                throw new UnauthorizedException("Invalid token");
            }

            var user = storedToken.User;
            storedToken.RevokedAt = DateTime.UtcNow;
            var (ip, ua, device) = GetClientInfo();
            storedToken.RevokedByIp = ip;
            storedToken.ReasonRevoked = "Rotated";

            var newRefreshToken = await _tokenService.CreateRefreshToken(user.Id, storedToken.SessionId, ip, ua, device);
            storedToken.ReplacedByToken = newRefreshToken;
            var newAccessToken = await _tokenService.CreateToken(user);

            await refreshTokenRepo.UpdateAsync(storedToken);
            await _unitOfWork.SaveChangesAsync();

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email!,
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        public async Task<UserDto> RegisterAsync(RegisterDto register)
        {
            var existingUser = await _userManager.FindByEmailAsync(register.Email);
            if (existingUser != null)
            {
                throw new ConflictException("Email is already taken");
            }

            var user = new AppUser
            {
                DisplayName = register.DisplayName,
                Email = register.Email,
                UserName = register.Email
            };

            var result = await _userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded)
            {
                throw new BadRequestException(result.Errors.First().Description);
            }

            await _userManager.AddToRoleAsync(user, UserRole.User.ToString());

            var accessToken = await _tokenService.CreateToken(user);
            var (ip, ua, device) = GetClientInfo();
            var newSessionId = Guid.NewGuid();
            var refreshToken = await _tokenService.CreateRefreshToken(user.Id, newSessionId, ip, ua, device);

            await EnforceMaxSessionsAsync(user.Id, ip);

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email!,
                Token = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task LogoutAsync(string refreshToken)
        {
            var refreshTokenRepo = _unitOfWork.Repository<RefreshToken>();
            var storedToken = await refreshTokenRepo.FirstOrDefaultAsync(new RefreshTokenWithUserSpecification(refreshToken));
            if (storedToken != null)
            {
                var (ip, _, __) = GetClientInfo();
                storedToken.RevokedAt = DateTime.UtcNow;
                storedToken.RevokedByIp = ip;
                await refreshTokenRepo.UpdateAsync(storedToken);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task LogoutAllAsync(string userId)
        {
            var refreshTokenRepo = _unitOfWork.Repository<RefreshToken>();
            var tokens = await refreshTokenRepo.ListAsync(new RefreshTokenSpecification(userId));

            if (tokens.Any())
            {
                var (ip, _, __) = GetClientInfo();
                // Update all tokens in memory first
                foreach (var token in tokens)
                {
                    token.RevokedAt = DateTime.UtcNow;
                    token.RevokedByIp = ip;
                    await refreshTokenRepo.UpdateAsync(token);
                }
                // Save all changes in a single batch transaction
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<UserDto> GetCurrentUser(ClaimsPrincipal claimsPrincipal)
        {
            var user = await _userManager.FindByEmailFromClaimsPrinciple(claimsPrincipal);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            return new UserDto
            {
                DisplayName = user!.DisplayName,
                Email = user.Email!,
            };
        }

        public async Task<IReadOnlyList<UserSessionDto>> GetSessionsAsync(string userId)
        {
            var refreshTokenRepo = _unitOfWork.Repository<RefreshToken>();
            var tokens = await refreshTokenRepo.ListAsync(new RefreshTokenSpecification(userId));
            var result = tokens
                .Select(t => new UserSessionDto
                {
                    SessionId = t.SessionId,
                    CreatedAt = t.CreatedAt,
                    LastUsedAt = t.LastUsedAt,
                    Expires = t.Expires,
                    CreatedByIp = t.CreatedByIp,
                    UserAgent = t.UserAgent,
                    DeviceName = t.DeviceName,
                    Active = t.IsActive
                })
                .ToList();
            return result;
        }

        public async Task RevokeSessionAsync(string userId, Guid sessionId, string? reason = null)
        {
            var refreshTokenRepo = _unitOfWork.Repository<RefreshToken>();
            var tokens = await refreshTokenRepo.ListAsync(new RefreshTokenSpecification(userId));
            var (ip, _, __) = GetClientInfo();
            var tokensToRevoke = tokens.Where(t => t.SessionId == sessionId).ToList();

            // Update all tokens in memory first
            foreach (var t in tokensToRevoke)
            {
                t.RevokedAt = DateTime.UtcNow;
                t.RevokedByIp = ip;
                t.ReasonRevoked = reason ?? "User revoked";
                await refreshTokenRepo.UpdateAsync(t);
            }

            // Save all changes in a single batch transaction
            if (tokensToRevoke.Any())
            {
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task RevokeOtherSessionsAsync(string userId, Guid keepSessionId)
        {
            var refreshTokenRepo = _unitOfWork.Repository<RefreshToken>();
            var tokens = await refreshTokenRepo.ListAsync(new RefreshTokenSpecification(userId));
            var (ip, _, __) = GetClientInfo();
            var tokensToRevoke = tokens.Where(t => t.SessionId != keepSessionId).ToList();

            // Update all tokens in memory first
            foreach (var t in tokensToRevoke)
            {
                t.RevokedAt = DateTime.UtcNow;
                t.RevokedByIp = ip;
                t.ReasonRevoked = "User revoked others";
                await refreshTokenRepo.UpdateAsync(t);
            }

            // Save all changes in a single batch transaction
            if (tokensToRevoke.Any())
            {
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<Guid?> GetSessionIdByRefreshTokenAsync(string refreshToken)
        {
            var refreshTokenRepo = _unitOfWork.Repository<RefreshToken>();
            var token = await refreshTokenRepo.FirstOrDefaultAsync(new RefreshTokenWithUserSpecification(refreshToken));
            return token?.SessionId;
        }

        private async Task EnforceMaxSessionsAsync(string userId, string? ip)
        {
            var refreshTokenRepo = _unitOfWork.Repository<RefreshToken>();
            var max = _tokenOptions.MaxSessionsPerUser;
            if (max <= 0) return;
            var active = await refreshTokenRepo.ListAsync(new RefreshTokenSpecification(userId));
            if (active.Count <= max) return;
            var toRevoke = active
                .OrderBy(t => t.LastUsedAt ?? t.CreatedAt)
                .Take(active.Count - max)
                .ToList();

            // Update all tokens in memory first
            foreach (var t in toRevoke)
            {
                t.RevokedAt = DateTime.UtcNow;
                t.RevokedByIp = ip;
                t.ReasonRevoked = "Max sessions exceeded";
                await refreshTokenRepo.UpdateAsync(t);
            }

            // Save all changes in a single batch transaction
            if (toRevoke.Any())
            {
                await _unitOfWork.SaveChangesAsync();
            }
        }

        private (string? ip, string? userAgent, string? device) GetClientInfo()
        {
            var http = _httpContextAccessor.HttpContext;
            var ip = http?.Connection?.RemoteIpAddress?.ToString();
            var userAgent = http?.Request?.Headers["User-Agent"].ToString();
            var device = http?.Request?.Headers["X-Device-Name"].ToString();
            return (ip, userAgent, string.IsNullOrWhiteSpace(device) ? null : device);
        }
    }
}
