using Core.Entities.Identity;
using Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {

            builder.Property(s => s.Name).HasMaxLength(100);
            builder.Property(s => s.Action).HasMaxLength(100);
            builder.Property(s => s.Module).HasConversion(
                            o => o.ToString(),
                            o => (AppModule)Enum.Parse(typeof(AppModule), o)
                            );

        }
    }
}
