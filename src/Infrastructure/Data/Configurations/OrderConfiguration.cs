using Core.Entities.OrderAggregate;
using Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(o => o.ShipToAddress, a =>
            {
                a.WithOwner();
                a.Property(s => s.FirstName)
                    .HasMaxLength(100)
                    .IsRequired();

                a.Property(s => s.LastName)
                    .HasMaxLength(100)
                    .IsRequired();

                a.Property(s => s.Street)
                        .HasMaxLength(100)
                        .IsRequired();

                a.Property(s => s.Ward)
                        .HasMaxLength(100)
                        .IsRequired();

                a.Property(s => s.District)
                    .HasMaxLength(100)
                    .IsRequired(); ;

                a.Property(s => s.City)
                        .HasMaxLength(100)
                        .IsRequired();

                a.Property(s => s.HouseNumber)
                        .HasMaxLength(100)
                        .IsRequired();
            });

            builder.Navigation(o => o.ShipToAddress).IsRequired();

            builder.Property(s => s.Status).HasConversion(
                o => o.ToString(),
                o => (OrderStatus)Enum.Parse(typeof(OrderStatus), o)
                );

            builder.HasMany(o => o.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
