using System;

namespace Core.Dtos
{
    public class UserSessionDto
    {
        public Guid SessionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUsedAt { get; set; }
        public DateTime Expires { get; set; }
        public string? CreatedByIp { get; set; }
        public string? UserAgent { get; set; }
        public string? DeviceName { get; set; }
        public bool Active { get; set; }
    }
}
