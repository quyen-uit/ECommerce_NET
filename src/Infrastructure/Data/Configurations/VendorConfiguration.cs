using Core.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
    {
        public void Configure(EntityTypeBuilder<Vendor> builder)
        {

            builder.Property(s => s.Name).HasMaxLength(100);
            builder.Property(s => s.Address).HasMaxLength(100);
            builder.Property(s => s.Email).HasMaxLength(100);
            builder.Property(s => s.Phone).HasMaxLength(100);

        }
    }
}
