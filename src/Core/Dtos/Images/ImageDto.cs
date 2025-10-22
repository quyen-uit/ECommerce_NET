namespace Core.Dtos.Images
{
    public class ImageDto
    {
        public long Id { get; set; }
        public required string Url { get; set; }
        public int Order { get; set; }
    }
}
