using System.Net;

namespace API.Commons
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

        public static ApiErrorResponse Fail(int statusCode, string? message = null, object? error = null)
        {
            return new ApiErrorResponse(statusCode, message, error);
        }
    }
}
