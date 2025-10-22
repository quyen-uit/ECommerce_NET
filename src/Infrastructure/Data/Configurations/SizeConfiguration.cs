using Core.Entities;
using Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class SizeConfiguration : IEntityTypeConfiguration<Size>
    {
        public void Configure(EntityTypeBuilder<Size> builder)
        {

            builder.Property(s => s.Name)
                    .HasMaxLength(100);
            builder.Property(s => s.SizeType).HasConversion(
                o => o.ToString(),
                o => (SizeType)Enum.Parse(typeof(SizeType), o)
                );

        }
    }
}
