using Core.Enums;

namespace Core.Dtos.Images
{
    public class CreateListImageDto
    {
        public Guid ReferenceId { get; set; }
        public ICollection<CreateImageDto> CreateImageDtos { get; set; } = new List<CreateImageDto>();


    }
    public class CreateImageDto
    {
        public Guid? Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public int Order { get; set; }
        public ImageType Type { get; set; }

    }

}
