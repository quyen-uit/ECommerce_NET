using System.Net;

namespace API.Commons
{
    public class ApiSuccessResponse<T> : ApiResponse
    {
        public T? Data { get; }

        public ApiSuccessResponse(T? data, int statusCode = (int)HttpStatusCode.OK, string? message = null)
            : base(statusCode, message)
        {
            Succeeded = true;
            Data = data;
        }
    }
}
