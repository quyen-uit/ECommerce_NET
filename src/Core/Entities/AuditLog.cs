using Core.Common.Entities;

namespace Core.Entities
{
    public class AuditLog : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public string Action { get; set; } = string.Empty; // HTTP method
        public string Path { get; set; } = string.Empty;
        public string? QueryString { get; set; }
        public int StatusCode { get; set; }
        public long DurationMs { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
