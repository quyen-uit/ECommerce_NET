namespace API.Commons
{
    public class ApiErrorResponse : ApiResponse
    {
        public object? Error { get; }

        public ApiErrorResponse(int statusCode, string? message = null, object? error = null)
            : base(statusCode, message)
        {
            Succeeded = false;
            Error = error;
        }
    }
}
