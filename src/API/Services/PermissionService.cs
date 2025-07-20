//using API.Helpers;
//using Core.Entities.Identity;
//using Core.Interfaces.Reposiories;
//using Core.Interfaces.Services;
//using Infrastructure.Data;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Caching.Memory;
//using System.Security.Claims;

//namespace API.Services
//{
//    public class PermissionService : IPermissionService
//    {
//        private readonly IGenericRepository<Permission> _permissionRepo;
//        private readonly UserManager<IdentityUser> _userManager;
//        private readonly IMemoryCache _cache;

//        public PermissionService(
//            IGenericRepository<Permission> permissionRepo,
//            UserManager<IdentityUser> userManager,
//            IMemoryCache cache)
//        {
//            _permissionRepo = permissionRepo;
//            _userManager = userManager;
//            _cache = cache;
//        }

//        public async Task<bool> UserHasPermissionAsync(string userId, string permission)
//        {
//            var cacheKey = $"user_permissions_{userId}";

//            if (!_cache.TryGetValue(cacheKey, out List<string> userPermissions))
//            {
//                userPermissions = await GetUserPermissionsAsync(userId);
//                _cache.Set(cacheKey, userPermissions, TimeSpan.FromMinutes(30));
//            }

//            return userPermissions.Contains(permission);
//        }

//        public async Task<List<string>> GetUserPermissionsAsync(string userId)
//        {
//            var user = await _userManager.FindByIdAsync(userId);
//            if (user == null) return new List<string>();

//            var userRoles = await _userManager.GetRolesAsync(user);

//            var rolePermissions = await _rolePermissionRepo.GetAllAsync(
//                rp => userRoles.Contains(rp.Role.Name),
//                include: query => query.Include(rp => rp.Permission).Include(rp => rp.Role));

//            return rolePermissions
//                .Select(rp => rp.Permission.Name)
//                .Distinct()
//                .ToList();
//        }

//        public async Task<bool> RoleHasPermissionAsync(string roleName, string permission)
//        {
//            var rolePermission = await _rolePermissionRepo.GetFirstOrDefaultAsync(
//                rp => rp.Role.Name == roleName && rp.Permission.Name == permission,
//                include: query => query.Include(rp => rp.Role).Include(rp => rp.Permission));

//            return rolePermission != null;
//        }
//    }
//}


//public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
//{
//    private readonly IServiceProvider _serviceProvider;

//    public PermissionAuthorizationHandler(IServiceProvider serviceProvider)
//    {
//        _serviceProvider = serviceProvider;
//    }

//    protected override async Task HandleRequirementAsync(
//        AuthorizationHandlerContext context,
//        PermissionRequirement requirement)
//    {
//        using var scope = _serviceProvider.CreateScope();
//        var permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();

//        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//        if (userId == null) return;

//        var hasPermission = await permissionService.UserHasPermissionAsync(userId, requirement.Permission);

//        if (hasPermission)
//        {
//            context.Succeed(requirement);
//        }
//    }
//}

//blic void ConfigureServices(IServiceCollection services)
//{
//    services.AddIdentity<IdentityUser, ApplicationRole>()
//        .AddEntityFrameworkStores<ApplicationDbContext>();

//services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

//services.AddAuthorization(options =>
//{
//    // Dynamically register permission policies
//    var permissions = GetAllPermissions(); // Method to get all permissions
//    foreach (var permission in permissions)
//    {
//        options.AddPolicy($"Permission.{permission}", policy =>
//            policy.Requirements.Add(new PermissionRequirement(permission)));
//    }
//});
//}

//public class RequirePermissionAttribute : AuthorizeAttribute
//{
//    public RequirePermissionAttribute(string permission)
//        : base($"Permission.{permission}")
//    {
//    }
//}

//// Usage in controllers
//[RequirePermission("Users.Create")]
//public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
//{
//    // Implementation
//}

//[RequirePermission("Products.Read")]
//public async Task<IActionResult> GetProducts()
//{
//    // Implementation
//}

//public class TokenService
//{
//    private readonly UserManager<IdentityUser> _userManager;
//    private readonly ApplicationDbContext _context;
//    private readonly IConfiguration _configuration;

//    public async Task<string> GenerateTokenAsync(IdentityUser user)
//    {
//        var userRoles = await _userManager.GetRolesAsync(user);

//        // Get all permissions for user's roles
//        var permissions = await _context.RolePermissions
//            .Include(rp => rp.Permission)
//            .Where(rp => userRoles.Contains(rp.Role.Name))
//            .Select(rp => rp.Permission.Name)
//            .Distinct()
//            .ToListAsync();

//        var claims = new List<Claim>
//        {
//            new(ClaimTypes.NameIdentifier, user.Id),
//            new(ClaimTypes.Name, user.UserName),
//            new(ClaimTypes.Email, user.Email)
//        };

//        // Add role claims
//        foreach (var role in userRoles)
//        {
//            claims.Add(new Claim(ClaimTypes.Role, role));
//        }

//        // Add permission claims
//        foreach (var permission in permissions)
//        {
//            claims.Add(new Claim("permission", permission));
//        }

//        // Generate JWT token...
//    }
//}