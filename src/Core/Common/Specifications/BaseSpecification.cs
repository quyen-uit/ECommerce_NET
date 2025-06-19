using System.Linq.Expressions;
using Core.Interfaces;
using Core.Specifications.Products;
using Microsoft.EntityFrameworkCore.Query;

namespace Core.Common.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification()
        {
        }

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        public Expression<Func<T, bool>> Criteria { get; } = null!;

        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

        public List<Func<IQueryable<T>, IIncludableQueryable<T, object>>> QueryableIncludes { get; } = new List<Func<IQueryable<T>, IIncludableQueryable<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; private set; } = null!;

        public Expression<Func<T, object>> OrderByDescending { get; private set; } = null!;

        public List<string> IncludeStrings { get; } = new List<string>();

        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPagingEnable { get; private set; }

        protected void AddInclude(Expression<Func<T, object>> include)
        {
            Includes.Add(include);
        }
        protected void AddQueryableInclude(Func<IQueryable<T>, IIncludableQueryable<T, object>> include)
        {
            QueryableIncludes.Add(include);
        }

        protected void AddIncludeString(string includeString)
        {
            IncludeStrings.Add(includeString);
        }

        protected void AddOrderBy(Expression<Func<T, object>> orderBy)
        {
            OrderBy = orderBy;
        }

        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescending)
        {
            OrderByDescending = orderByDescending;
        }

        protected void AddPagination(int pageSize, int pageNumber)
        {
            Skip = pageSize * (pageNumber - 1);
            Take = pageSize;
            IsPagingEnable = true;
        }
    }
}
