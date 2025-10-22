using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace API.Middlewares
{
    public class RefreshRateLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;
        private readonly ILogger<RefreshRateLimitMiddleware> _logger;
        private readonly int _limitPerMinute;

        private static readonly PathString RefreshPath = new("/api/account/refresh");

        public RefreshRateLimitMiddleware(RequestDelegate next, IMemoryCache cache, IConfiguration config, ILogger<RefreshRateLimitMiddleware> logger)
        {
            _next = next;
            _cache = cache;
            _logger = logger;
            _limitPerMinute = int.TryParse(config["Security:RefreshRateLimitPerMinute"], out var v) && v > 0 ? v : 10;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (HttpMethods.IsPost(context.Request.Method) && context.Request.Path.Equals(RefreshPath, StringComparison.OrdinalIgnoreCase))
            {
                var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                var key = $"rrl:{ip}";
                var count = _cache.GetOrCreate<int>(key, entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
                    return 0;
                });

                if (count >= _limitPerMinute)
                {
                    _logger.LogWarning("Rate limited refresh for IP {IP}", ip);
                    context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    context.Response.ContentType = "application/json";
                    var payload = JsonSerializer.Serialize(new { message = "Too many refresh requests. Try again later." });
                    await context.Response.WriteAsync(payload);
                    return;
                }

                _cache.Set(key, count + 1, TimeSpan.FromMinutes(1));
            }

            await _next(context);
        }
    }
}
