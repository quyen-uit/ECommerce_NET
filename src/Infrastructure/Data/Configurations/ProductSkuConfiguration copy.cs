using Core.Entities;
using Core.Entities.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class StoreProductSkuConfiguration : IEntityTypeConfiguration<StoreProductSku>
    {
        public void Configure(EntityTypeBuilder<StoreProductSku> builder)
        {
            builder.HasKey(si => new { si.StoreId, si.ProductSkuId }); // Composite key

            builder.HasOne(sp => sp.Store)
                .WithMany(s => s.StoreProductSkus)
                .HasForeignKey(si => si.StoreId);

            builder.HasOne(sp => sp.ProductSku)
                .WithMany(ps => ps.StoreProductSkus)
                .HasForeignKey(si => si.ProductSkuId);
        }
    }
}
