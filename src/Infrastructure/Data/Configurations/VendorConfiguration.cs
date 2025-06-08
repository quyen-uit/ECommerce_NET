using Core.Entities.Inventory;
using Core.Entities.OrderAggregate;
using Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configurations
{
    public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
    {
        public void Configure(EntityTypeBuilder<Vendor> builder)
        {

            builder.Property(s => s.Name).HasMaxLength(100).IsRequired();
            builder.Property(s => s.Address).HasMaxLength(100).IsRequired();
            builder.Property(s => s.Email).HasMaxLength(100).IsRequired();
            builder.Property(s => s.Phone).HasMaxLength(100).IsRequired();

        }
    }
}
