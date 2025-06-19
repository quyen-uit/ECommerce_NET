using Core.Common.Entities;
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

            var  queryInclude = specification.Includes.Aggregate(query, (current,include) =>  current.Include(include));
            var  queryQueryableInclude = specification.QueryableIncludes.Aggregate(queryInclude, (current,include) =>  include(current));

            var resultQuery = specification.IncludeStrings.Aggregate(queryQueryableInclude, (current, include) => current.Include(include));

            return resultQuery;
        }
    }
}
