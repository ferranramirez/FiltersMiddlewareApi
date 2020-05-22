using Microsoft.AspNetCore.Builder;
using System;

namespace FiltersApi.MiddleWare.Exception
{
    public static class ApiExceptionMiddlewareExtension
    {
        public static IApplicationBuilder UseApiExceptionHandler(this IApplicationBuilder builder)
        {
            var options = new ApiExceptionOptions();
            return builder.UseMiddleware<MyFilterExceptionMiddleware>(options);
        }

        public static IApplicationBuilder UseApiExceptionHandler(this IApplicationBuilder builder,
            Action<ApiExceptionOptions> configureOptions)
        {
            //    var options = new ApiExceptionOptions();
            //    configureOptions(options);

            return builder.UseMiddleware<MyFilterExceptionMiddleware>();
        }
    }
}
