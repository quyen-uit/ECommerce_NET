using Ardalis.Specification;
using Core.Entities;

namespace Core.Specifications.Products;

public class ProductsByIdsSpecification : Specification<Product>
{
    public ProductsByIdsSpecification(IEnumerable<Guid> productIds)
    {
        Query.Where(p => productIds.Contains(p.Id));
    }
}
