using Core.Common.Entities;
using Core.Interfaces;
using Core.Interfaces.Reposiories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T>
        where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async void Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public async void AddRange(IReadOnlyList<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<int> CountAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).CountAsync();
        }

        public async Task DeleteById(long id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
            }
        }
        public void Delete(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Deleted;
        }

        public void DeleteRange(List<T> values)
        {
            if (values != null && values.Count > 0)
            {
                foreach (var entity in values)
                {
                    _context.Set<T>().Attach(entity);
                    _context.Entry(entity).State = EntityState.Deleted;
                }
            }

        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).ToListAsync();
        }

        public async Task<T?> GetByIdAsync(long id)
        {
            return await _context
                .Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(entity => entity.Id == id);
        }

        public async Task<T?> GetEntityWithSpecAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync();
        }

        public void Update(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> specification)
        {
            return SpecificationEvaluator<T>.GetQuery(
                _context.Set<T>().AsQueryable<T>(),
                specification
            );
        }
        public async Task DeleteRangeById(List<long> ids)
        {
            var entities = await _context.Set<T>().Where(x => ids.Contains(x.Id)).ToListAsync();
            if (entities != null && entities.Count > 0)
            {
                _context.Set<T>().RemoveRange(entities);
            }
        }

        public async void SoftDeleteById(long id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null && !entity.IsDeleted)
            {
                entity.IsDeleted = true;
                _context.Set<T>().Update(entity);
            }
        }

        public async Task SoftDeleteRangeById(List<long> ids)
        {
            var entities = await _context.Set<T>().Where(x => ids.Contains(x.Id)).ToListAsync();
            if (entities != null && entities.Count > 0)
            {
                foreach (var entity in entities)
                {
                    entity.IsDeleted = true;
                }
                _context.Set<T>().UpdateRange(entities);
            }
        }
    }
}
