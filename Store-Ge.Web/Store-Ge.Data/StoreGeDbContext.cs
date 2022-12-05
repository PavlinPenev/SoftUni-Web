using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Store_Ge.Data.Models;

using static Store_Ge.Common.Constants.CommonConstants;

namespace Store_Ge.Data
{
    public class StoreGeDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
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
            base.OnModelCreating(builder);

            // Order Model Creating
            builder.Entity<Order>()
                .HasOne(o => o.Supplier)
                .WithMany(s => s.Orders)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Order>()
                .HasOne(o => o.Store)
                .WithMany(s => s.Orders)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Order>()
                .HasMany(o => o.Products)
                .WithMany(p => p.Orders);

            // Store Model Creating
            builder.Entity<Store>()
                .HasMany(s => s.Orders)
                .WithOne(o => o.Store)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Store>()
                .HasMany(s => s.AuditEvents)
                .WithOne(ae => ae.Store)
                .OnDelete(DeleteBehavior.Restrict);

            // Product Model Creating
            builder.Entity<Product>()
                .HasMany(p => p.Suppliers)
                .WithMany(s => s.Products);

            // Composite keys
            builder.Entity<Models.UserStore>()
                .HasKey(us => new { us.UserId, us.StoreId });

            builder.Entity<StoreSupplier>()
                .HasKey(ss => new { ss.StoreId, ss.SupplierId });

            builder.Entity<ApplicationRole>()
                .HasData(
                new ApplicationRole
                {
                    Id = 1,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new ApplicationRole
                {
                    Id = 2,
                    Name = "Cashier",
                    NormalizedName = "CASHIER"
                });
        }

        private void ApplyAuditInfoRules()
        {
            var changedEntries = this.ChangeTracker
                .Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in changedEntries)
            {
                var entryProperty = entry.State == EntityState.Added
                    ? entry.Entity.GetType().GetProperty(CREATED_ON_PROPERTY_STRING_LITERAL)
                    : entry.Entity.GetType().GetProperty(MODIFIED_ON_PROPERTY_STRING_LITERAL);

                if (entryProperty != null)
                {
                    entryProperty.SetValue(entry.Entity, DateTime.UtcNow);
                }
            }
        }
    }
} 