using Core.Entities;
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
    public class ColorConfiguration : IEntityTypeConfiguration<Color>
    {
        public void Configure(EntityTypeBuilder<Color> builder)
        {

            builder.Property(s => s.Name)
                    .HasMaxLength(100)
                    .IsRequired();
            builder.Property(s => s.HexCode)
                    .HasMaxLength(100)
                    .IsRequired();

        }
    }
}
