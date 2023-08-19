namespace API.Errors
{
    public class ApiValidationErrorResponse : ApiResponse
    {
        public ApiValidationErrorResponse() : base(statusCode: 400)
        {
        }
        public IEnumerable<string> Errors { get; set; }
    }
}
