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
            // builder.Navigation(o => o.ShipToAddress).IsRequired();

            builder.Property(s => s.Status).HasConversion(
                o => o.ToString(),
                o => (OrderStatus)Enum.Parse(typeof(OrderStatus), o)
                );

            builder.HasMany(o => o.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);

            // Indexes - Critical Priority
            // Index 1: User's order history (composite)
            builder.HasIndex(o => new { o.BuyerEmail, o.OrderDate })
                .HasDatabaseName("IX_Orders_BuyerEmail_OrderDate")
                .IsDescending(false, true) // Email ASC, OrderDate DESC
                .HasFilter("\"IsDeleted\" = false");

            // Index 2: Admin order filtering by status
            builder.HasIndex(o => new { o.Status, o.OrderDate })
                .HasDatabaseName("IX_Orders_Status_OrderDate")
                .IsDescending(false, true)
                .HasFilter("\"IsDeleted\" = false");

            // Index 3: Payment intent lookup (Stripe webhooks)
            builder.HasIndex(o => o.PaymentIntentId)
                .HasDatabaseName("IX_Orders_PaymentIntentId")
                .HasFilter("\"IsDeleted\" = false AND \"PaymentIntentId\" IS NOT NULL");

            // Index 4: Date range queries
            builder.HasIndex(o => o.OrderDate)
                .HasDatabaseName("IX_Orders_OrderDate")
                .IsDescending()
                .HasFilter("\"IsDeleted\" = false");
        }
    }
}
