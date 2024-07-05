using Core.Common;
using Core.Entities;

namespace Core.Specifications.Products
{
    public class ProductTypesWithActiveFilterSpecification : BaseSpecification<Category>
    {
        public ProductTypesWithActiveFilterSpecification(bool isActive)
            : base(x => x.IsActive == isActive)
        {

        }

    }
}
