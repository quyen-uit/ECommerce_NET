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
        }
    }
}
