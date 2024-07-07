using Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.CreateDto
{
    public class ProductColorDto
    {
        public int ColorId { get; set; }
        public Color Color { get; set; }

        public List<string> PhotoUrls { get; set; }

    }

}
