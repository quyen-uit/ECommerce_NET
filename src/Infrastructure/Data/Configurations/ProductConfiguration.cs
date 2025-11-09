using Core.Entities;
using Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder
                .HasMany(p => p.Collections)
                .WithMany(c => c.Products)
                .UsingEntity(j => j.ToTable("ProductCollections"));

            builder
                .Property(p => p.Name)
                .HasMaxLength(100);

            builder
                .Property(p => p.Description);

            builder
               .HasMany(rv => rv.Reviews)
               .WithOne(p => p.Product)
               .HasForeignKey(rv => rv.ProductId);

            builder
                .OwnsMany(p => p.Properties, builder => builder.ToJson());

            builder.Property(s => s.GenderType).HasConversion(
                o => o.ToString(),
                o => (GenderType)Enum.Parse(typeof(GenderType), o)
                );

            // Indexes - Critical Priority
            // Index 1: Foreign key - CategoryId
            builder.HasIndex(p => p.CategoryId)
                .HasDatabaseName("IX_Products_CategoryId")
                .HasFilter("\"IsDeleted\" = false");

            // Index 2: Foreign key - ProductBrandId
            builder.HasIndex(p => p.ProductBrandId)
                .HasDatabaseName("IX_Products_ProductBrandId")
                .HasFilter("\"IsDeleted\" = false");

            // Index 3: Name search (LIKE queries)
            builder.HasIndex(p => p.Name)
                .HasDatabaseName("IX_Products_Name")
                .HasFilter("\"IsDeleted\" = false");

            // Index 4: Boolean filters (composite)
            builder.HasIndex(p => new { p.IsNew, p.IsTrending, p.IsActive })
                .HasDatabaseName("IX_Products_IsNew_IsTrending_IsActive")
                .HasFilter("\"IsDeleted\" = false");

            // Index 5: Default sorting - UpdatedAt DESC
            builder.HasIndex(p => p.UpdatedAt)
                .HasDatabaseName("IX_Products_UpdatedAt")
                .IsDescending()
                .HasFilter("\"IsDeleted\" = false");
        }
    }
}
