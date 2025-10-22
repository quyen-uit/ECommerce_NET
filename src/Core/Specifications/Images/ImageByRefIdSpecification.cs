using Core.Common.Specifications;
using Core.Entities;
using Core.Enums;

namespace Core.Specifications.Images
{
    public class ImageByRefIdSpecification : BaseSpecification<Image>
    {
        public ImageByRefIdSpecification(long refId, ImageType type)
            : base(x => x.ReferenceId == refId && x.Type == type)
        {
            AddOrderBy(x => x.Order);
        }
    }
}
