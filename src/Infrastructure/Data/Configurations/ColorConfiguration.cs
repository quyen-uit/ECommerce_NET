using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ColorConfiguration : IEntityTypeConfiguration<Color>
    {
        public void Configure(EntityTypeBuilder<Color> builder)
        {

            builder.Property(s => s.Name)
                    .HasMaxLength(100);
            builder.Property(s => s.HexCode)
                    .HasMaxLength(100);

        }
    }
}
