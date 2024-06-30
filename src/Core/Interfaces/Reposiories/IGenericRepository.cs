using Core.Common;

namespace Core.Interfaces.Reposiories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> GetEntityWithSpecAsync(ISpecification<T> specification);
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> specification);
        Task<int> CountAsync(ISpecification<T> specification);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<int> Complete();
    }
}
