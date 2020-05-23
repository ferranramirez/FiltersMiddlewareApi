using Business.ExceptionFilter;
using FiltersApi.Filters.Exception;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;

namespace FiltersApi.MiddleWare.Exception
{
    public class MyFilterExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MyFilterExceptionMiddleware> _logger;

        public MyFilterExceptionMiddleware(
            RequestDelegate next,
            ILogger<MyFilterExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                using (_logger.BeginScope("using scope: ParentThrowSomeException()"))
                using (_logger.BeginScope("User", "NoUser"))
                {
                    _logger.LogInformation("Super information");

                    await _next(context);
                }

                _logger.LogInformation("After");
            }
            catch (MyFilterException ex) when (LogWarning(ex))
            {
                HandleExceptionAsync(context, ex);
            }
            catch (System.Exception ex) when (LogError(ex))
            {
                HandleExceptionAsync(context, ex);
            }
        }

        private bool LogWarning(System.Exception ex)
        {
            _logger.LogWarning(
                new EventId(0),
                ex,
                $"Application thrown error: {ex.Message}");

            return true;
        }
        private bool LogError(System.Exception ex)
        {
            _logger.LogError(
                new EventId(0),
                ex,
                $"Unhandled exception: {ex.Message}");
            return true;
        }

        private Task HandleExceptionAsync(HttpContext context, System.Exception exception)
        {
            var apiError = new ApiError();

            if (exception is MyFilterException)
            {
                var ex = exception as MyFilterException;

                apiError.Message = ex.Message;

                apiError.ErrorCode = ex.ErrorCode;

                context.Response.StatusCode = 543;
            }
            else
            {
                apiError.Message = exception.GetBaseException().Message;

                apiError.ErrorCode = (int)HttpStatusCode.InternalServerError;

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            var result = JsonConvert.SerializeObject(apiError);

            return context.Response.WriteAsync(result);
        }
    }
}
