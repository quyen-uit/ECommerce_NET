using Core.Entities.Inventory;
using Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class InventoryTransactionConfiguration : IEntityTypeConfiguration<InventoryTransaction>
    {
        public void Configure(EntityTypeBuilder<InventoryTransaction> builder)
        {

            builder.Property(s => s.TransactionType).HasConversion(
                o => o.ToString(),
                o => (InventoryTransactionType)Enum.Parse(typeof(InventoryTransactionType), o)
            );
        }
    }
}
