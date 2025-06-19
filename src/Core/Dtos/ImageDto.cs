using System.ComponentModel.DataAnnotations;
using Core.Enums;

namespace Core.Dtos
{
    public class ImageDto
    {
        public long Id { get; set; }
        public required string Url { get; set; }
        public int Order { get; set; }
    }

    public class CreateOrUpdateImageDto
    {
        public long? Id { get; set; }
        [Required]
        public required string Url { get; set; }
        public int Order { get; set; }
        public bool IsDelete { get; set; } = false;
        public ImageType Type { get; set; }

    }

}
