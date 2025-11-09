using Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            // Primary key
            builder.HasKey(rt => rt.Id);

            // Relationship to User
            builder.HasOne(rt => rt.User)
                .WithMany()
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Property configurations
            builder.Property(rt => rt.Token)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(rt => rt.UserId)
                .IsRequired();

            // Indexes - Critical Priority
            // Index 1: Token lookup - UNIQUE (MOST CRITICAL!)
            builder.HasIndex(rt => rt.Token)
                .HasDatabaseName("IX_RefreshTokens_Token")
                .IsUnique()
                .HasFilter("\"RevokedAt\" IS NULL");

            // Index 2: User's active sessions
            builder.HasIndex(rt => new { rt.UserId, rt.RevokedAt })
                .HasDatabaseName("IX_RefreshTokens_UserId_RevokedAt")
                .HasFilter("\"RevokedAt\" IS NULL");

            // Index 3: Session management
            builder.HasIndex(rt => rt.SessionId)
                .HasDatabaseName("IX_RefreshTokens_SessionId")
                .HasFilter("\"RevokedAt\" IS NULL");

            // Index 4: Cleanup expired tokens
            builder.HasIndex(rt => rt.Expires)
                .HasDatabaseName("IX_RefreshTokens_Expires")
                .HasFilter("\"RevokedAt\" IS NULL");
        }
    }
}
