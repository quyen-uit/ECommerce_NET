using Core.Common.Entities;

namespace Core.Interfaces.Reposiories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task<T?> GetEntityWithSpecAsync(ISpecification<T> specification);
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> specification);
        Task<int> CountAsync(ISpecification<T> specification);
        void Add(T entity);
        void Update(T entity);
        void AddRange(IReadOnlyList<T> entities);
        Task DeleteById(Guid id);
        void Delete(T entity);
        void DeleteRange(List<T> entity);
        Task DeleteRangeById(List<Guid> ids);

        void SoftDeleteById(Guid id);
        Task SoftDeleteRangeById(List<Guid> ids);
        Task<int> Complete();
    }
}
