using API.Commons.Response;
using Core.Exceptions;
using API.Helpers;
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
            var stackTrace = env.IsDevelopment() ? exception.StackTrace : null;
            ApiResponse response;

            switch (exception)
            {
                case ValidationException validationException:
                    statusCode = (int)HttpStatusCode.UnprocessableEntity;
                    response = ResponseFactory.Fail(statusCode, validationException.Message, validationException.Errors, stackTrace);
                    break;
                case NotFoundException notFoundException:
                    statusCode = (int)HttpStatusCode.NotFound;
                    response = ResponseFactory.Fail(statusCode, notFoundException.Message, detail: stackTrace);
                    break;
                case BadRequestException badRequestException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    response = ResponseFactory.Fail(statusCode, badRequestException.Message, detail: stackTrace);
                    break;
                case UnauthorizedException unauthorizedException:
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    response = ResponseFactory.Fail(statusCode, unauthorizedException.Message, detail: stackTrace);
                    break;
                case ForbiddenException forbiddenException:
                    statusCode = (int)HttpStatusCode.Forbidden;
                    response = ResponseFactory.Fail(statusCode, forbiddenException.Message, detail: stackTrace);
                    break;
                case ConflictException conflictException:
                    statusCode = (int)HttpStatusCode.Conflict;
                    response = ResponseFactory.Fail(statusCode, conflictException.Message, detail: stackTrace);
                    break;
                default:
                    var safeMessage = env.IsDevelopment() ? exception.Message : "An unexpected error occurred";
                    response = ResponseFactory.Fail(statusCode, safeMessage, detail: stackTrace);
                    break;
            }

            context.Response.StatusCode = statusCode;
            return Task.FromResult(response);
        }
    }
}
