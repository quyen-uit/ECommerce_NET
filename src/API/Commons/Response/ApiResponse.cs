using System.Net;

namespace API.Commons.Response
{
    public abstract class ApiResponse
    {
        public string Message { get; protected set; } = null!;
        public int StatusCode { get; protected set; }
        public bool Succeeded { get; protected set; }

        public ApiResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private static string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                (int)HttpStatusCode.OK => "Success",
                (int)HttpStatusCode.Created => "Created",
                (int)HttpStatusCode.NoContent => "No Content",
                (int)HttpStatusCode.BadRequest => "Bad Request",
                (int)HttpStatusCode.Unauthorized => "Unauthorized",
                (int)HttpStatusCode.Forbidden => "Forbidden",
                (int)HttpStatusCode.NotFound => "Not Found",
                (int)HttpStatusCode.InternalServerError => "Internal Server Error",
                _ => string.Empty,
            };
        }
    }
}
