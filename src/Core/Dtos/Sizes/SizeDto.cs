namespace Core.Dtos.Sizes
{
    public class SizeDto
    {
        public long Id { get; set; }
        public required string Name { get; set; }
        public int SortOrder { get; set; }
        public string? SizeType { get; set; }
    }

}
