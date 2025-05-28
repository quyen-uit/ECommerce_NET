using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder
                .Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder
                .Property(p => p.Description)
                .IsRequired();

            builder
               .HasMany(rv => rv.Reviews)
               .WithOne(p => p.Product)
               .HasForeignKey(rv => rv.ProductId);

            builder
                .OwnsMany(p => p.Properties, builder => builder.ToJson());

        }
    }
}
