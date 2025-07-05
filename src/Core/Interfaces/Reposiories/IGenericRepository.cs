using Core.Common.Entities;

namespace Core.Interfaces.Reposiories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T?> GetByIdAsync(long id);
        Task<T?> GetEntityWithSpecAsync(ISpecification<T> specification);
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> specification);
        Task<int> CountAsync(ISpecification<T> specification);
        void Add(T entity);
        void Update(T entity);
        void AddRange(IReadOnlyList<T> entities);
        Task DeleteById(long id);
        void Delete(T entity);
        void DeleteRange(List<T> entity);
        Task DeleteRangeById(List<long> ids);

        void SoftDeleteById(long id);
        Task SoftDeleteRangeById(List<long> ids);
        Task<int> Complete();
    }
}
