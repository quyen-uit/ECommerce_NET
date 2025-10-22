namespace API.Commons.Response
{
    public class ApiError
    {
        public int StatusCode { get; set; }
        public required string Message { get; set; }
        public string? Details { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
