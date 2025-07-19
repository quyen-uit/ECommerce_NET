using API.Exceptions;
using API.Extensions;
using Core.Dtos;
using Core.Entities.Identity;
using Core.Interfaces.Reposiories;
using Core.Interfaces.Services;
using Core.Specifications.Accounts;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace API.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IGenericRepository<RefreshToken> _refreshTokenRepository;

        public AccountService(UserManager<AppUser> userManager, ITokenService tokenService, IGenericRepository<RefreshToken> refreshTokenRepository)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<UserDto> LoginAsync(LoginDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                throw new UnauthorizedException("Invalid email or password");
            }

            var accessToken = await _tokenService.CreateToken(user);
            var refreshToken = await _tokenService.CreateRefreshToken(user.Id);

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
            var storedToken = await _refreshTokenRepository.GetEntityWithSpecAsync(new RefreshTokenWithUserSpecification(token));
            if (storedToken == null)
            {
                throw new UnauthorizedException("Invalid token");
            }

            var user = storedToken.User;
            storedToken.RevokedAt = DateTime.UtcNow;

            var newRefreshToken = await _tokenService.CreateRefreshToken(user.Id);
            var newAccessToken = await _tokenService.CreateToken(user);

            _refreshTokenRepository.Update(storedToken);
            await _refreshTokenRepository.Complete();

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
            var refreshToken = await _tokenService.CreateRefreshToken(user.Id);

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
            var storedToken = await _refreshTokenRepository.GetEntityWithSpecAsync(new RefreshTokenWithUserSpecification(refreshToken));
            if (storedToken != null)
            {
                storedToken.RevokedAt = DateTime.UtcNow;
                _refreshTokenRepository.Update(storedToken);
                await _refreshTokenRepository.Complete();
            }
        }

        public async Task LogoutAllAsync(string userId)
        {
            var tokens = await _refreshTokenRepository.GetAllWithSpecAsync(new RefreshTokenSpecification(userId));

            if (tokens.Any())
            {
                foreach (var token in tokens)
                {
                    token.RevokedAt = DateTime.UtcNow;
                    _refreshTokenRepository.Update(token);
                }
                await _refreshTokenRepository.Complete();
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
    }
}
