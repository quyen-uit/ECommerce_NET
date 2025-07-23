using Core.Common.Entities;
using Core.Interfaces.Reposiories;

namespace Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        IRolePermissionRepository RolePermissionRepository { get; }
        Task<int> Complete();
    }
}
