using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                .Property(p => p.Price)
                .HasPrecision(14, 2);

            builder
                .HasOne(pt => pt.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.ProductTypeId)
                .OnDelete(DeleteBehavior.Restrict);


            builder
               .HasOne(pb => pb.ProductBrand)
               .WithMany(p => p.Products)
               .HasForeignKey(p => p.ProductBrandId)
               .OnDelete(DeleteBehavior.Restrict);

            builder
               .HasMany(rv => rv.Reviews)
               .WithOne(p => p.Product)
               .HasForeignKey(rv => rv.ProductId);

        }
    }
}
