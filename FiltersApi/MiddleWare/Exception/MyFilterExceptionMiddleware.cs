using Business.ExceptionFilter;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
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
            catch (System.Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext filterContext, System.Exception exception)
        {
            string message;

            if (exception is MyFilterException)
            {
                var ex = exception as MyFilterException;

                message = ex.Message;

                filterContext.Response.StatusCode = 543;

                _logger.LogWarning($"Application thrown error: {message}", ex);
            }
            else if (exception is UnauthorizedAccessException)
            {
                var ex = exception as UnauthorizedAccessException;

                message = ex.Message;

                filterContext.Response.StatusCode = 444;

                _logger.LogWarning($"Application thrown error: {message}", ex);
            }
            else
            {
                message = exception.GetBaseException().Message;

                filterContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                _logger.LogError($"Unhandled exception: {message}", exception.StackTrace);
            }

            var result = JsonConvert.SerializeObject(message);

            return filterContext.Response.WriteAsync(result);
        }
    }
}
