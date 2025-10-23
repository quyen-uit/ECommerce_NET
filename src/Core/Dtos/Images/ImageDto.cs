namespace Core.Dtos.Images
{
    public class ImageDto
    {
        public Guid Id { get; set; }
        public required string Url { get; set; }
        public int Order { get; set; }
    }
}
