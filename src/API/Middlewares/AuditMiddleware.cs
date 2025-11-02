using Core.Entities;
using Infrastructure.Data;
using System.Diagnostics;

namespace API.Middlewares
{
    public class AuditMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuditMiddleware> _logger;

        // Paths that should not be audited (for performance)
        private static readonly string[] ExcludedPaths = new[]
        {
            "/health",
            "/swagger",
            "/_framework",
            "/api/v1/basket" // High-frequency endpoint
        };

        public AuditMiddleware(RequestDelegate next, ILogger<AuditMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, ApplicationDbContext dbContext)
        {
            // Skip audit for excluded paths
            if (ShouldSkipAudit(context.Request.Path))
            {
                await _next(context);
                return;
            }

            var stopwatch = Stopwatch.StartNew();
            var userId = context.User?.Identity?.IsAuthenticated == true
                ? context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "Anonymous"
                : "Anonymous";
            var userName = context.User?.Identity?.Name;

            try
            {
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();

                // Only audit authenticated requests or important endpoints
                if (context.User?.Identity?.IsAuthenticated == true || IsImportantEndpoint(context.Request.Path))
                {
                    try
                    {
                        var auditLog = new AuditLog
                        {
                            UserId = userId,
                            UserName = userName,
                            Action = context.Request.Method,
                            Path = context.Request.Path,
                            QueryString = context.Request.QueryString.HasValue ? context.Request.QueryString.Value : null,
                            StatusCode = context.Response.StatusCode,
                            DurationMs = stopwatch.ElapsedMilliseconds,
                            IpAddress = context.Connection.RemoteIpAddress?.ToString(),
                            UserAgent = context.Request.Headers["User-Agent"].ToString(),
                            Timestamp = DateTime.UtcNow
                        };

                        dbContext.AuditLogs.Add(auditLog);
                        await dbContext.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        // Don't fail the request if audit logging fails
                        _logger.LogError(ex, "Failed to write audit log");
                    }
                }
            }
        }

        private static bool ShouldSkipAudit(PathString path)
        {
            foreach (var excludedPath in ExcludedPaths)
            {
                if (path.StartsWithSegments(excludedPath, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        private static bool IsImportantEndpoint(PathString path)
        {
            // Audit login, logout, and sensitive operations even if not authenticated
            return path.StartsWithSegments("/api/v1/account/login", StringComparison.OrdinalIgnoreCase)
                || path.StartsWithSegments("/api/v1/account/logout", StringComparison.OrdinalIgnoreCase)
                || path.StartsWithSegments("/api/v1/account/register", StringComparison.OrdinalIgnoreCase);
        }
    }
}
