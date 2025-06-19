using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Core.Common.Specifications;
using Core.Entities;
using Core.Enums;
using Core.Specifications.Products;

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
