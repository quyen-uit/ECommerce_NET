using Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.Property(s => s.FirstName)
                .HasMaxLength(100);

            builder.Property(s => s.LastName)
                .HasMaxLength(100);

            builder.Property(s => s.Street)
                .HasMaxLength(100);

            builder.Property(s => s.Ward)
                .HasMaxLength(100);

            builder.Property(s => s.District)
                .HasMaxLength(100);

            builder.Property(s => s.City)
                .HasMaxLength(100);

            builder.Property(s => s.HouseNumber)
                .HasMaxLength(100);

        }
    }
}
