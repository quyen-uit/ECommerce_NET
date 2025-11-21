using Core.Interfaces.Services;
// using Microsoft.Extensions.Caching.Distributed; // Commented out - Redis disabled
using System.Text.Json;

namespace API.Services
{
    // PermissionCacheService - Commented out - Uses Redis (via IDistributedCache)
    // public class PermissionCacheService : IPermissionCacheService
    // {
    //     private readonly IDistributedCache _distributedCache;
    //     private readonly ILogger<PermissionCacheService> _logger;
    //     private const string CacheKeyPrefix = "perm:";
    //     private const string AllKeysPattern = "perm:*";
    //     private static readonly TimeSpan DefaultExpiration = TimeSpan.FromHours(1);
    //
    //     public PermissionCacheService(IDistributedCache distributedCache, ILogger<PermissionCacheService> logger)
    //     {
    //         _distributedCache = distributedCache;
    //         _logger = logger;
    //     }
    //
    //     public async Task<HashSet<string>?> GetPermissionsAsync(string userId)
    //     {
    //         try
    //         {
    //             var key = GetCacheKey(userId);
    //             var cached = await _distributedCache.GetStringAsync(key);
    //
    //             if (string.IsNullOrEmpty(cached))
    //                 return null;
    //
    //             return JsonSerializer.Deserialize<HashSet<string>>(cached);
    //         }
    //         catch (Exception ex)
    //         {
    //             _logger.LogError(ex, "Error retrieving permissions from cache for user {UserId}", userId);
    //             return null;
    //         }
    //     }
    //
    //     public async Task SetPermissionsAsync(string userId, HashSet<string> permissions, TimeSpan? expiration = null)
    //     {
    //         try
    //         {
    //             var key = GetCacheKey(userId);
    //             var serialized = JsonSerializer.Serialize(permissions);
    //             var options = new DistributedCacheEntryOptions
    //             {
    //                 AbsoluteExpirationRelativeToNow = expiration ?? DefaultExpiration
    //             };
    //
    //             await _distributedCache.SetStringAsync(key, serialized, options);
    //         }
    //         catch (Exception ex)
    //         {
    //             _logger.LogError(ex, "Error setting permissions in cache for user {UserId}", userId);
    //         }
    //     }
    //
    //     public async Task InvalidatePermissionsAsync(string userId)
    //     {
    //         try
    //         {
    //             var key = GetCacheKey(userId);
    //             await _distributedCache.RemoveAsync(key);
    //         }
    //         catch (Exception ex)
    //         {
    //             _logger.LogError(ex, "Error invalidating permissions cache for user {UserId}", userId);
    //         }
    //     }
    //
    //     public async Task InvalidateAllPermissionsAsync()
    //     {
    //         try
    //         {
    //             // Note: This is a simplified implementation
    //             // For production with Redis, consider using SCAN with pattern matching
    //             // or maintaining a set of all user IDs to invalidate
    //             _logger.LogWarning("InvalidateAllPermissionsAsync called. This is a heavy operation. Consider using Redis SCAN or maintaining user ID sets for better performance.");
    //
    //             // For now, we'll just log a warning
    //             // In a real implementation, you'd use IConnectionMultiplexer to access Redis directly
    //             // and use SCAN with the pattern "perm:*" to find and delete keys
    //         }
    //         catch (Exception ex)
    //         {
    //             _logger.LogError(ex, "Error invalidating all permissions cache");
    //         }
    //     }
    //
    //     private static string GetCacheKey(string userId)
    //     {
    //         return $"{CacheKeyPrefix}{userId}";
    //     }
    // }
}
