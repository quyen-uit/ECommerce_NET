using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            // Primary key
            builder.HasKey(a => a.Id);

            // Property configurations
            builder.Property(a => a.UserId)
                .IsRequired()
                .HasMaxLength(450); // Match ASP.NET Identity user ID length

            builder.Property(a => a.Action)
                .IsRequired()
                .HasMaxLength(10); // HTTP methods: GET, POST, PUT, DELETE, PATCH

            builder.Property(a => a.Path)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(a => a.QueryString)
                .HasMaxLength(2000);

            builder.Property(a => a.IpAddress)
                .HasMaxLength(45); // IPv6 max length

            builder.Property(a => a.UserAgent)
                .HasMaxLength(500);

            // Indexes - Critical Priority
            // Index 1: User activity tracking
            builder.HasIndex(a => new { a.UserId, a.Timestamp })
                .HasDatabaseName("IX_AuditLogs_UserId_Timestamp")
                .IsDescending(false, true); // UserId ASC, Timestamp DESC

            // Index 2: Endpoint monitoring
            builder.HasIndex(a => new { a.Path, a.Timestamp })
                .HasDatabaseName("IX_AuditLogs_Path_Timestamp")
                .IsDescending(false, true);

            // Index 3: HTTP method filtering
            builder.HasIndex(a => new { a.Action, a.Timestamp })
                .HasDatabaseName("IX_AuditLogs_Action_Timestamp")
                .IsDescending(false, true);

            // Index 4: Time-based queries (most common)
            builder.HasIndex(a => a.Timestamp)
                .HasDatabaseName("IX_AuditLogs_Timestamp")
                .IsDescending();
        }
    }
}
