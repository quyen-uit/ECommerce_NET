using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ProductSkuConfiguration : IEntityTypeConfiguration<ProductSku>
    {
        public void Configure(EntityTypeBuilder<ProductSku> builder)
        {
            builder
                .Property(p => p.SkuCode)
                .HasMaxLength(50);

            // Indexes - High Priority
            // Index 1: Product's variants
            builder.HasIndex(ps => ps.ProductId)
                .HasDatabaseName("IX_ProductSkus_ProductId")
                .HasFilter("\"IsDeleted\" = false");

            // Index 2: Color/Size filtering
            builder.HasIndex(ps => new { ps.ColorId, ps.SizeId })
                .HasDatabaseName("IX_ProductSkus_ColorId_SizeId")
                .HasFilter("\"IsDeleted\" = false");

            // Index 3: SKU code - UNIQUE
            builder.HasIndex(ps => ps.SkuCode)
                .HasDatabaseName("IX_ProductSkus_SkuCode")
                .IsUnique()
                .HasFilter("\"IsDeleted\" = false");
        }
    }
}
