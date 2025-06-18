using Core.Entities.ReturnOrder;
using Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ReturnOrderConfiguration : IEntityTypeConfiguration<ReturnOrder>
    {
        public void Configure(EntityTypeBuilder<ReturnOrder> builder)
        {

            builder.Property(o => o.TotalAmount).HasPrecision(18, 2);
            builder.Property(s => s.Status).HasConversion(
                o => o.ToString(),
                o => (ReturnOrderStatus)Enum.Parse(typeof(ReturnOrderStatus), o)
                );
        }
    }
}
