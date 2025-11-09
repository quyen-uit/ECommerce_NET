using Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {

            builder.HasKey(rp => new { rp.RoleId, rp.PermissionId });

            builder
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes - High Priority (RBAC Performance)
            // Index 1: Permission lookup by role (CRITICAL for permission resolution)
            builder.HasIndex(rp => rp.RoleId)
                .HasDatabaseName("IX_RolePermissions_RoleId");

            // Index 2: Reverse lookup (which roles have a permission)
            builder.HasIndex(rp => rp.PermissionId)
                .HasDatabaseName("IX_RolePermissions_PermissionId");

            // Index 3: Composite unique constraint (explicit)
            builder.HasIndex(rp => new { rp.RoleId, rp.PermissionId })
                .HasDatabaseName("IX_RolePermissions_RoleId_PermissionId")
                .IsUnique();
        }
    }
}
