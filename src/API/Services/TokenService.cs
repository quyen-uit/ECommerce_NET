using Core.Entities.Identity;
using Ardalis.Specification;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        private readonly IRepositoryBase<RefreshToken> _refreshTokenRepository;
        private readonly UserManager<AppUser> _userManager;
        public TokenService(IConfiguration config, IRepositoryBase<RefreshToken> refreshTokenRepository, UserManager<AppUser> userManager)
        {
            _config = config;
            _refreshTokenRepository = refreshTokenRepository;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"]!));
            _userManager = userManager;
        }


        public async Task<string> CreateToken(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.GivenName, user.DisplayName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(_config["Token:AccessTokenExpiration"] ?? "15")),
                SigningCredentials = credentials,
                Issuer = _config["Token:Issuer"],
                Audience = _config["Token:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }


        public async Task<string> CreateRefreshToken(string userId, Guid? sessionId = null, string? createdByIp = null, string? userAgent = null, string? deviceName = null)

        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.UtcNow.AddDays(int.Parse(_config["Token:RefreshTokenExpirationDays"] ?? "7")),
                CreatedAt = DateTime.UtcNow,
                LastUsedAt = DateTime.UtcNow,
                UserId = userId,
                SessionId = sessionId ?? Guid.NewGuid(),
                CreatedByIp = createdByIp,
                UserAgent = userAgent,
                DeviceName = deviceName
            };

            await _refreshTokenRepository.AddAsync(refreshToken);

            return refreshToken.Token;
        }


    }
}
