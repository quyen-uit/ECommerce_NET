using Core.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Color : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string HexCode { get; set; } = string.Empty;

    }

}
