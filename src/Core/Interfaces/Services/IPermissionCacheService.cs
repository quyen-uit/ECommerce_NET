namespace Core.Interfaces.Services
{
    public interface IPermissionCacheService
    {
        Task<HashSet<string>?> GetPermissionsAsync(string userId);
        Task SetPermissionsAsync(string userId, HashSet<string> permissions, TimeSpan? expiration = null);
        Task InvalidatePermissionsAsync(string userId);
        Task InvalidateAllPermissionsAsync();
    }
}
