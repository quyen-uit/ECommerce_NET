using Core.Entities;
using Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class PriceAdjustmentConfiguration : IEntityTypeConfiguration<PriceAdjustment>
    {
        public void Configure(EntityTypeBuilder<PriceAdjustment> builder)
        {
            builder.Property(a => a.Name).HasMaxLength(100);
            builder.HasMany(a => a.PriceAdjustmentItems)
                    .WithOne(a => a.PriceAdjustment)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
