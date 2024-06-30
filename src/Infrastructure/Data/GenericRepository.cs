﻿using Core.Common;
using Core.Interfaces;
using Core.Interfaces.Reposiories;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
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

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<int> CountAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).CountAsync();
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return  await _context.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> specification)
        {
            return await ApplySpecification(specification).ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> GetEntityWithSpecAsync(ISpecification<T> specification)
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
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable<T>(), specification);
        }
    }
}
