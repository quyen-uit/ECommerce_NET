using System.ComponentModel.DataAnnotations;
using Core.Enums;

namespace Core.Dtos.Images
{
    public class CreateListImageDto
    {
        [Required]
        public long ReferenceId { get; set; }
        public ICollection<CreateImageDto> CreateImageDtos { get; set; } = new List<CreateImageDto>();
         

    }
    public class CreateImageDto
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public required string Url { get; set; }
        public int Order { get; set; }
        public ImageType Type { get; set; }

    }

}
