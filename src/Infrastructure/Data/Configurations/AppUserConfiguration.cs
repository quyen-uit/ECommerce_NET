using Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder
                .HasOne(u => u.Address)
                .WithOne(a => a.AppUser);

            builder.Property(a => a.DisplayName).HasMaxLength(100);

            builder.OwnsOne(o => o.Address, a =>
            {
                a.WithOwner();
                a.Property(s => s.FirstName)
                    .HasMaxLength(100);

                a.Property(s => s.LastName)
                    .HasMaxLength(100);

                a.Property(s => s.Street)
                        .HasMaxLength(100);

                a.Property(s => s.Ward)
                        .HasMaxLength(100);

                a.Property(s => s.District)
                    .HasMaxLength(100);

                a.Property(s => s.City)
                        .HasMaxLength(100);

                a.Property(s => s.HouseNumber)
                        .HasMaxLength(100);
            });

        }
    }
}
