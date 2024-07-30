namespace API.Errors
{
    public class ApiException : ApiResponse
    {
        public ApiException(int statusCode, string message = null, string detail = null) : base(statusCode, message)
        {
            Detail = detail;
        }
        public string Detail { get; set; }
    }
}
