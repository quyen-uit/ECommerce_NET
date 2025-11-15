using Core.Exceptions;

namespace Core.Dtos.Sizes
{
    public class CreateSizeDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public string SizeType { get; set; } = string.Empty;
    }
}
