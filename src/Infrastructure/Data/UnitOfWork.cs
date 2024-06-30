using Core.Common;
using Core.Interfaces;
using Core.Interfaces.Reposiories;
using System.Collections;


namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        private Hashtable _repositories;
        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            if (_repositories == null)
            {
                _repositories = new Hashtable();
            }

            var entityType = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(entityType))
            {
                var repoType = typeof(GenericRepository<>);
                var repositoryInstance = Activator.CreateInstance(repoType.MakeGenericType(typeof(TEntity)), _context);
                _repositories.Add(entityType, repositoryInstance);
            }

            return (IGenericRepository<TEntity>) _repositories[entityType];
        }
    }
}
