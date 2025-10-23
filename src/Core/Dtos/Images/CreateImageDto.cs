using Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.Images
{
    public class CreateListImageDto
    {
        [Required]
        public Guid ReferenceId { get; set; }
        public ICollection<CreateImageDto> CreateImageDtos { get; set; } = new List<CreateImageDto>();


    }
    public class CreateImageDto
    {
        public Guid? Id { get; set; }
        [Required]
        public required string Url { get; set; }
        public int Order { get; set; }
        public ImageType Type { get; set; }

    }

}
