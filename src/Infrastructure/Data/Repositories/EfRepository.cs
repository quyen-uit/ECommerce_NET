using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Infrastructure.Data;
using Core.Interfaces.Reposiories;

namespace Infrastructure.Data.Repositories
{
    public class EfRepository<T> : RepositoryBase<T>, IRepository<T>, Ardalis.Specification.IRepositoryBase<T> where T : class
    {
        public EfRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}


