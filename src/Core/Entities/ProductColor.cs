using Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class ProductColor : BaseEntity
    {
        public Color Color { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public int ColorId { get; set; }

        public List<string> PhotoUrls { get; set; }
    }
}
