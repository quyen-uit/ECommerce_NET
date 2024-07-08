using Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.CreateDto
{
    public class ProductColorDto
    {
        public int ColorId { get; set; }
        public string ColorName { get; set; }
        public string HexCode { get; set; }
        public List<string> PhotoUrls { get; set; }

    }

}
