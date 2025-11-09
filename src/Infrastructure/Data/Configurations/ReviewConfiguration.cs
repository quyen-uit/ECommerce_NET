using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            // Primary key
            builder.HasKey(r => r.Id);

            // Relationships
            builder.HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.AppUser)
                .WithMany()
                .HasForeignKey(r => r.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Property configurations
            builder.Property(r => r.Rating)
                .IsRequired();

            builder.Property(r => r.Comment)
                .IsRequired()
                .HasMaxLength(2000);

            // Indexes - High Priority
            // Index 1: Product's reviews (sorted by date)
            builder.HasIndex(r => new { r.ProductId, r.CreatedAt })
                .HasDatabaseName("IX_Reviews_ProductId_CreatedAt")
                .IsDescending(false, true)
                .HasFilter("\"IsDeleted\" = false");

            // Index 2: User's reviews
            builder.HasIndex(r => r.AppUserId)
                .HasDatabaseName("IX_Reviews_AppUserId")
                .HasFilter("\"IsDeleted\" = false");
        }
    }
}
