using Core.Entities;
using Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {

            builder.Property(s => s.Type).HasConversion(
                o => o.ToString(),
                o => (ImageType)Enum.Parse(typeof(ImageType), o)
                    );
        }
    }
}
