using Microsoft.AspNetCore.Builder;

namespace FiltersApi.MiddleWare.Exception
{
    public static class ApiExceptionMiddlewareExtension
    {
        public static IApplicationBuilder UseApiExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MyFilterExceptionMiddleware>();
        }
    }
}
