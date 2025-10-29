using Ardalis.Specification;
using Core.Entities;
using Core.Enums;

namespace Core.Specifications.Images
{
    public class ImageByRefIdSpecification : Specification<Image>
    {
        public ImageByRefIdSpecification(Guid refId, ImageType type)
        {
            Query.Where(x => x.ReferenceId == refId && x.Type == type)
                 .OrderBy(x => x.Order);
        }
    }
}
