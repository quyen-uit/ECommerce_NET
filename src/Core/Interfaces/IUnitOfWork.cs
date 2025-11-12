using Core.Interfaces.Reposiories;

namespace Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets a repository for the specified entity type.
        /// Repositories are cached per UnitOfWork instance.
        /// </summary>
        IRepository<T> Repository<T>() where T : class;

        /// <summary>
        /// Saves all changes made in this unit of work to the database.
        /// This operation is automatically wrapped in a transaction by EF Core.
        /// </summary>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Saves all changes made in this unit of work to the database.
        /// </summary>
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
    }
}
