using Microsoft.EntityFrameworkCore;

namespace Store_Ge.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        public Repository(StoreGeDbContext context)
        {
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
            this.DbSet = this.Context.Set<TEntity>();
        }

        protected StoreGeDbContext Context { get; set; }

        protected DbSet<TEntity> DbSet { get; set; }

        public virtual Task AddAsync(TEntity entity) => this.DbSet.AddAsync(entity).AsTask();

        public void Delete(TEntity entity) => this.DbSet.Remove(entity);

        public virtual IQueryable<TEntity> GetAll() => this.DbSet;

        public Task<int> SaveChangesAsync() => this.Context.SaveChangesAsync();

        public virtual async Task BulkMerge(ICollection<TEntity> items) => await this.Context.BulkMergeAsync(items);

        public void Update(TEntity entity)
        {
            var entry = this.Context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.DbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Context?.Dispose();
            }
        }
    }
}
