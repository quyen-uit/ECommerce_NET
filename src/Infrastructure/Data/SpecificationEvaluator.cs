using Core.Common;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Data
{
    public static class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification) 
        {
            var query = inputQuery;

            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }
            
            if (specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            
            if (specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }
            
            if (specification.IsPagingEnable)
            {
                query = query.Skip(specification.Skip).Take(specification.Take);
            }

            var  queryFirstInclude = specification.Includes.Aggregate(query, (current,include) =>  current.Include(include));

            var resultQuery = specification.IncludeStrings.Aggregate(queryFirstInclude, (current, include) => current.Include(include));

            return resultQuery;
        }
    }
}
