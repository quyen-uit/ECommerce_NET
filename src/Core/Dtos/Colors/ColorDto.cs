namespace Core.Dtos.Colors
{
    public class ColorDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string HexCode { get; set; }
    }
}
