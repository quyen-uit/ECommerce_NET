using System.Text.Json;

namespace API.Middlewares
{
    public class SpaOriginValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SpaOriginValidationMiddleware> _logger;
        private readonly string _allowedOrigin;

        // Paths that require origin/referer validation
        private static readonly PathString[] ProtectedPaths = new[]
        {
            new PathString("/api/account/refresh"),
            new PathString("/api/account/logout"),
            new PathString("/api/account/sessions"), // includes subpaths
        };

        public SpaOriginValidationMiddleware(RequestDelegate next, IConfiguration config, ILogger<SpaOriginValidationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            _allowedOrigin = config["Spa:Origin"] ?? "https://localhost:4200";
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (HttpMethods.IsPost(context.Request.Method) && IsProtectedPath(context.Request.Path))
            {
                var origin = context.Request.Headers["Origin"].ToString();
                var referer = context.Request.Headers["Referer"].ToString();
                var candidate = !string.IsNullOrEmpty(origin) ? origin : ExtractOrigin(referer);

                if (string.IsNullOrEmpty(candidate) || !string.Equals(candidate, _allowedOrigin, StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogWarning("Blocked request due to invalid origin/referer. Origin={Origin} Referer={Referer}", origin, referer);
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    context.Response.ContentType = "application/json";
                    var payload = JsonSerializer.Serialize(new { message = "Forbidden: invalid origin" });
                    await context.Response.WriteAsync(payload);
                    return;
                }
            }

            await _next(context);
        }

        private static bool IsProtectedPath(PathString path)
        {
            foreach (var p in ProtectedPaths)
            {
                if (path.StartsWithSegments(p, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        private static string ExtractOrigin(string referer)
        {
            if (string.IsNullOrEmpty(referer)) return string.Empty;
            if (Uri.TryCreate(referer, UriKind.Absolute, out var uri))
            {
                var port = uri.IsDefaultPort ? string.Empty : $":{uri.Port}";
                return $"{uri.Scheme}://{uri.Host}{port}";
            }
            return string.Empty;
        }
    }
}
