using Core.Exceptions;

namespace Core.Dtos.Colors
{

    public class CreateColorDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string HexCode { get; set; } = string.Empty;
    }

}
