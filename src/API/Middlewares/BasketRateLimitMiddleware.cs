using API.Helpers;
using API.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace API.Middlewares
{
    public class BasketRateLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;
        private readonly ILogger<BasketRateLimitMiddleware> _logger;
        private readonly int _limitPerMinute;

        private static readonly PathString BasketPath = new("/api/basket");

        public BasketRateLimitMiddleware(RequestDelegate next, IMemoryCache cache, IOptions<SecurityOptions> securityOptions, ILogger<BasketRateLimitMiddleware> logger)
        {
            _next = next;
            _cache = cache;
            _logger = logger;
            _limitPerMinute = securityOptions.Value.BasketRateLimitPerMinute;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Rate limit all basket operations (GET, POST, DELETE)
            if (context.Request.Path.StartsWithSegments(BasketPath, StringComparison.OrdinalIgnoreCase))
            {
                var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                var key = $"brl:{ip}";
                var count = _cache.GetOrCreate<int>(key, entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                    return 0;
                });

                if (count >= _limitPerMinute)
                {
                    _logger.LogWarning("Rate limited basket operations for IP {IP}", ip);
                    context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    context.Response.ContentType = "application/json";
                    var payload = JsonSerializer.Serialize(ResponseFactory.Fail(StatusCodes.Status429TooManyRequests, "Too many basket requests. Try again later."));
                    await context.Response.WriteAsync(payload);
                    return;
                }

                _cache.Set(key, count + 1, TimeSpan.FromMinutes(1));
            }

            await _next(context);
        }
    }
}
