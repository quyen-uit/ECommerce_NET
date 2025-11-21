using Core.Interfaces.Services;
using Core.Exceptions;
// using StackExchange.Redis; // Commented out - Redis disabled
using System.Text.Json;

namespace API.Services
{
    // ResponseCacheService - Commented out - Uses Redis
    // public class ResponseCacheService : IResponseCacheService
    // {
    //     private readonly IDatabase _database;
    //
    //     public ResponseCacheService(IConnectionMultiplexer redis)
    //     {
    //         _database = redis.GetDatabase();
    //     }
    //
    //     public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
    //     {
    //         if (response == null) return;
    //
    //         var option = new JsonSerializerOptions
    //         {
    //             PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    //         };
    //
    //         var serializedResponse = JsonSerializer.Serialize(response, option);
    //         await _database.StringSetAsync(cacheKey, serializedResponse, timeToLive);
    //     }
    //
    //     public async Task<string?> GetCacheResponseAsync(string cacheKey)
    //     {
    //         var cachedResponse = await _database.StringGetAsync(cacheKey);
    //         return cachedResponse;
    //     }
    // }
}
