using Ardalis.Specification;

namespace Core.Interfaces.Reposiories
{
    public interface IRepository<T> : IRepositoryBase<T> where T : class
    {
    }
}

