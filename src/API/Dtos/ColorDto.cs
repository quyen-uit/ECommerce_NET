using Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class ColorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HexCode { get; set; }

    }

}
