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
                await _next(context);
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

                _logger.LogWarning(
                    new EventId(0),
                    ex,
                    $"Application thrown error: {ex.Message}");
            }
            else
            {
                apiError.Message = exception.GetBaseException().Message;

                apiError.ErrorCode = (int)HttpStatusCode.InternalServerError;

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                _logger.LogError(
                    new EventId(0),
                    exception,
                    $"Unhandled exception: {exception.Message}");
            }

            var result = JsonConvert.SerializeObject(apiError);

            return context.Response.WriteAsync(result);
        }
    }
}
