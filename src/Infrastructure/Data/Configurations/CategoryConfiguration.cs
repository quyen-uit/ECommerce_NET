using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder
                .HasOne(pc => pc.Parent)
                .WithMany(p => p.SubCategories)
                .HasForeignKey(pc => pc.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(s => s.Name)
                .HasMaxLength(100);

            // Indexes - High Priority
            // Index 1: Hierarchical navigation
            builder.HasIndex(c => new { c.ParentId, c.Order })
                .HasDatabaseName("IX_Categories_ParentId_Order")
                .HasFilter("\"IsDeleted\" = false");

            // Index 2: Active categories filter
            builder.HasIndex(c => c.IsActive)
                .HasDatabaseName("IX_Categories_IsActive")
                .HasFilter("\"IsDeleted\" = false");
        }
    }
}
