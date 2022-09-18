using Microsoft.EntityFrameworkCore;
using Store_Ge.Data.Models;

namespace Store_Ge.Data
{
    public class StoreGeDbContext : DbContext
    {
        public StoreGeDbContext(DbContextOptions<StoreGeDbContext> options)
            : base(options)
        {
        }

        public override int SaveChanges() => this.SaveChanges(true);

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            this.SaveChangesAsync(true, cancellationToken);

        public override Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            this.ApplyAuditInfoRules();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Order Model Creating
            builder.Entity<Order>()
                .HasOne(o => o.Supplier)
                .WithMany(s => s.Orders)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Order>()
                .HasOne(o => o.Store)
                .WithMany(s => s.Orders)
                .OnDelete(DeleteBehavior.Restrict);

            // Store Model Creating
            builder.Entity<Store>()
                .HasMany(s => s.Orders)
                .WithOne(o => o.Store)
                .OnDelete(DeleteBehavior.Restrict);

            // Composite keys
            builder.Entity<OrderProduct>()
                .HasKey(op => new { op.OrderId, op.ProductId });

            builder.Entity<StoreProduct>()
                .HasKey(sp => new { sp.StoreId, sp.ProductId });

            builder.Entity<SupplierProduct>()
                .HasKey(sp => new { sp.SupplierId, sp.ProductId });

            builder.Entity<Models.UserStore>()
                .HasKey(us => new { us.UserId, us.StoreId });

            builder.Entity<StoreSupplier>()
                .HasKey(ss => new { ss.StoreId, ss.SupplierId });
        }
        private void ApplyAuditInfoRules()
        {
            var changedEntries = this.ChangeTracker
                .Entries()
                .Where(e =>
                    e.Entity is User &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in changedEntries)
            {
                var entity = (User)entry.Entity;
                if (entry.State == EntityState.Added && entity.CreatedOn == default)
                {
                    entity.CreatedOn = DateTime.UtcNow;
                }
                else
                {
                    entity.ModifiedOn = DateTime.UtcNow;
                }
            }
        }
    }
}