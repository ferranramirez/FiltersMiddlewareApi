using Business.ExceptionFilter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Net;

namespace FiltersApi.Filters.Exception
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        private ILogger<ApiExceptionFilter> _logger;

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext filterContext)
        {
            var exception = filterContext.Exception;
            var apiError = new ApiError();

            if (exception is MyFilterException)
            {
                var ex = exception as MyFilterException;

                filterContext.Exception = null;

                apiError.Message = ex.Message;

                apiError.ErrorCode = ex.ErrorCode;

                filterContext.HttpContext.Response.StatusCode = 543;

                _logger.LogWarning(
                    new EventId(0),
                    ex,
                    $"Application thrown error: {ex.Message}");

            }
            else
            {
                apiError.Message = filterContext.Exception.GetBaseException().Message;

                apiError.ErrorCode = (int)HttpStatusCode.InternalServerError;

                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                _logger.LogError(
                    new EventId(0),
                    exception,
                    $"Unhandled exception: {exception.Message}");
            }

            filterContext.Result = new JsonResult(apiError);

            filterContext.ExceptionHandled = true;
        }
    }
}
