using Core.Interfaces;
using Core.Interfaces.Reposiories;
using Infrastructure.Data.Repositories;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly Dictionary<Type, object> _repositories;
        private bool _disposed;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
        }

        /// <summary>
        /// Gets a repository for the specified entity type.
        /// Repositories are cached per UnitOfWork instance to ensure consistency.
        /// </summary>
        public IRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);

            if (_repositories.TryGetValue(type, out var existingRepo))
            {
                return (IRepository<T>)existingRepo;
            }

            var repository = new EfRepository<T>(_context);
            _repositories[type] = repository;
            return repository;
        }

        /// <summary>
        /// Saves all changes made in this unit of work to the database.
        /// EF Core automatically wraps this in a transaction - if any operation fails, all changes are rolled back.
        /// </summary>
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Saves all changes made in this unit of work to the database.
        /// </summary>
        public async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _repositories.Clear();
                _context.Dispose();
            }
            _disposed = true;
        }
    }
}
