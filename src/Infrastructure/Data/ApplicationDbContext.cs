using System.Reflection;
using Core.Common.Entities;
using Core.Entities;
using Core.Entities.Identity;
using Core.Entities.Inventory;
using Core.Entities.OrderAggregate;
using Core.Entities.ReturnOrder;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductBrand> ProductBrands => Set<ProductBrand>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> Items => Set<OrderItem>();
        public DbSet<DeliveryMethod> DeliveryMethods => Set<DeliveryMethod>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<Color> Colors => Set<Color>();
        public DbSet<Size> Sizes => Set<Size>();
        public DbSet<Vendor> Vendors => Set<Vendor>();
        public DbSet<InventoryImport> InventoryImports => Set<InventoryImport>();
        public DbSet<InventoryAdjustment> InventoryAdjustments => Set<InventoryAdjustment>();
        public DbSet<InventoryTransaction> InventoryTransactions => Set<InventoryTransaction>();
        public DbSet<ReturnOrder> ReturnOrders => Set<ReturnOrder>();
        public DbSet<ReturnOrderItem> ReturnOrderItems => Set<ReturnOrderItem>();
        public DbSet<Image> Images => Set<Image>();
        public DbSet<PriceAdjustment> PriceAdjustments => Set<PriceAdjustment>();
        public DbSet<ProductSku> ProductSkus => Set<ProductSku>();
        public DbSet<Collection> Collections => Set<Collection>();
        public DbSet<Wishlist> Wishlists => Set<Wishlist>();
        public DbSet<Address> Addresses => Set<Address>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder
                        .Entity(entityType.ClrType)
                        .Property("Id")
                        .UseIdentityByDefaultColumn()
                        .HasIdentityOptions(startValue: 10);
                }
            }

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override int SaveChanges()
        {
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedDatetime = now;
                }

                entry.Entity.UpdatedDatetime = now;
            }

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default
        )
        {
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedDatetime = now;
                }

                entry.Entity.UpdatedDatetime = now;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
