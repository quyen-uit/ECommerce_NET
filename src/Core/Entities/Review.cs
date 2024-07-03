using Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Review : BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int UserId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }

}
