using Core.Common;
using Core.Entities;
using Core.Entities.Identity;
using Core.Entities.Inventory;
using Core.Entities.OrderAggregate;
using Core.Entities.ReturnOrder;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductBrand> ProductBrands => Set<ProductBrand>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> Items => Set<OrderItem>();
        public DbSet<DeliveryMethod> DeliveryMethods => Set<DeliveryMethod>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<Color> Colors => Set<Color>();
        public DbSet<Size> Sizes => Set<Size>();
        public DbSet<Vendor> Vendors  => Set<Vendor>();
        public DbSet<InventoryImport> InventoryImports  => Set<InventoryImport>();
        public DbSet<InventoryAdjustment> InventoryAdjustments  => Set<InventoryAdjustment>();
        public DbSet<InventoryTransaction> InventoryTransactions  => Set<InventoryTransaction>();
        public DbSet<ReturnOrder> ReturnOrders  => Set<ReturnOrder>();
        public DbSet<ReturnOrderItem> ReturnOrderItems  => Set<ReturnOrderItem>();
        public DbSet<Image> Images => Set<Image>();
        public DbSet<PriceAdjustment> PriceAdjustments => Set<PriceAdjustment>();
        public DbSet<ProductSku> ProductSkus => Set<ProductSku>();
        public DbSet<Wishlist> Wishlists => Set<Wishlist>();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).UpdatedDate = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreatedDate = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }
    }
}
