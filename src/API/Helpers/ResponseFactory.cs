using System.Net;
using API.Commons.Response;

namespace API.Helpers
{
    public static class ResponseFactory
    {
        public static ApiSuccessResponse<T> Ok<T>(T data, int statusCode = (int)HttpStatusCode.OK, string? message = null)
        {
            return new ApiSuccessResponse<T>(data, statusCode, message);
        }

        public static ApiSuccessResponse<object> Ok(int statusCode = (int)HttpStatusCode.OK, string? message = null)
        {
            return new ApiSuccessResponse<object>(null, statusCode, message);
        }

        public static ApiErrorResponse Fail(int statusCode, string? message = null, IEnumerable<string>? error = null, string? detail = null)
        {
            return new ApiErrorResponse(statusCode, message, error, detail);
        }
    }
}
