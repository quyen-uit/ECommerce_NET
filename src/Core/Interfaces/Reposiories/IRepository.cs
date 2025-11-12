using Ardalis.Specification;

namespace Core.Interfaces.Reposiories
{
    /// <summary>
    /// Repository interface that supports Ardalis Specification pattern for queries.
    /// Write operations (Add/Update/Delete) do NOT auto-save - you must call SaveChangesAsync on UnitOfWork.
    /// </summary>
    public interface IRepository<T> : IRepositoryBase<T> where T : class
    {
        // Read operations with Specification support (inherited from IRepositoryBase<T>):
        // - Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default)
        // - Task<T?> FirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        // - Task<List<T>> ListAsync(CancellationToken cancellationToken = default)
        // - Task<List<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        // - Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        // - Task<int> CountAsync(CancellationToken cancellationToken = default)
        // - Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        // - Task<bool> AnyAsync(CancellationToken cancellationToken = default)

        // Write operations (NO auto-save - must call UnitOfWork.SaveChangesAsync):
        // - Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        // - Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        // - Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        // - Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        // - Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        // - Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    }
}

