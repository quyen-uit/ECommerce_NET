using API.Commons;
using API.Exceptions;
using System.Net;
using System.Text.Json;

namespace API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _hostEnvironment;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment hostEnvironment)
        {
            _next = next;
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                httpContext.Response.ContentType = "application/json";
                var response = await HandleExceptionAsync(httpContext, ex, _hostEnvironment);
                var json = JsonSerializer.Serialize(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                await httpContext.Response.WriteAsync(json);
            }
        }

        private static Task<ApiResponse> HandleExceptionAsync(HttpContext context, Exception exception, IHostEnvironment env)
        {
            var statusCode = (int)HttpStatusCode.InternalServerError;
            ApiResponse response;

            switch (exception)
            {
                case ValidationException validationException:
                    statusCode = (int)HttpStatusCode.UnprocessableEntity;
                    response = ResponseFactory.Fail(statusCode, validationException.Message, validationException.Errors);
                    break;
                case NotFoundException notFoundException:
                    statusCode = (int)HttpStatusCode.NotFound;
                    response = ResponseFactory.Fail(statusCode, notFoundException.Message);
                    break;
                case BadRequestException badRequestException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    response = ResponseFactory.Fail(statusCode, badRequestException.Message);
                    break;
                case UnauthorizedException unauthorizedException:
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    response = ResponseFactory.Fail(statusCode, unauthorizedException.Message);
                    break;
                case ForbiddenException forbiddenException:
                    statusCode = (int)HttpStatusCode.Forbidden;
                    response = ResponseFactory.Fail(statusCode, forbiddenException.Message);
                    break;
                case ConflictException conflictException:
                    statusCode = (int)HttpStatusCode.Conflict;
                    response = ResponseFactory.Fail(statusCode, conflictException.Message);
                    break;
                default:
                    response = ResponseFactory.Fail(statusCode, exception.Message, env.IsDevelopment() ? exception.StackTrace : null);
                    break;
            }

            context.Response.StatusCode = statusCode;
            return Task.FromResult(response);
        }
    }
}
