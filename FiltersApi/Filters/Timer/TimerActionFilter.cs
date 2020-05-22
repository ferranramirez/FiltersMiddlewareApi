using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace FiltersApi.Filters.Timer
{
    public class TimerActionFilter : IActionFilter
    {
        private Stopwatch _timer;
        private readonly ILogger<TimerActionFilter> _logger;

        public TimerActionFilter(ILogger<TimerActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _timer = new Stopwatch();
            _timer.Start();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _timer.Stop();
            _logger.LogInformation(context.HttpContext.Request.Path,
                    context.HttpContext.Request.Method,
                    _timer.ElapsedMilliseconds);
        }

    }
}
