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

            builder.Property(s => s.Name).IsRequired();
            builder.Property(s => s.Address).IsRequired();
            builder.Property(s => s.Email).IsRequired();
            builder.Property(s => s.Phone).IsRequired();

        }
    }
}
