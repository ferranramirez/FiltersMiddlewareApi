using Business.ExceptionFilter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

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
            string message;

            if (exception is MyFilterException)
            {
                var ex = exception as MyFilterException;

                filterContext.Exception = null;

                message = ex.Message;

                filterContext.HttpContext.Response.StatusCode = 543;

                _logger.LogWarning($"Application thrown error: {message}", ex);
            }
            else
            {
                message = filterContext.Exception.GetBaseException().Message;

                filterContext.HttpContext.Response.StatusCode = 500;

                _logger.LogError($"Unhandled exception: {message}", exception.StackTrace);
            }

            filterContext.Result = new JsonResult(message);

            filterContext.ExceptionHandled = true;
        }
    }
}
