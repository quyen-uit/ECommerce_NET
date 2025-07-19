namespace API.Commons.Response
{
    public class ApiErrorResponse : ApiResponse
    {
        public IEnumerable<string>? Error { get; init; }
        public string? Detail { get; init; }

        public ApiErrorResponse(int statusCode, string? message = null, IEnumerable<string>? error = null, string? detail = null)
            : base(statusCode, message)
        {
            Succeeded = false;
            Error = error;
            Detail = detail;
        }
    }
}
