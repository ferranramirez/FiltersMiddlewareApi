using Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FiltersApi.Controllers
{
    [ApiController]
    // in case we don't inject it in the startup with the addMvc function, to have it in all the controllers
    // [TypeFilter(typeof(ApiExceptionFilter))]
    // [TypeFilter(typeof(TimerActionFilter))]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private FiltersBusiness _filtersBusiness;

        public TestController(ILogger<TestController> logger, FiltersBusiness filtersBusiness)
        {
            _logger = logger;
            _filtersBusiness = filtersBusiness;
        }

        // [Authorize]
        [HttpGet]
        public IActionResult ParentThrowSomeException()
        {
            //_filtersBusiness.ParentThrowSomeException();

            return Ok("Generic result");
        }

        [HttpGet("NonHandled")]
        public IActionResult Get2()
        {
            _logger.LogInformation("Test endpoint NonHandled called");

            _filtersBusiness.ThrowSomeException();

            return Ok("Generic result");
        }

        [HttpGet("Auth")]
        public IActionResult AuthorizationTest()
        {
            _logger.LogInformation("Test endpoint AuthorizationTest called");

            _filtersBusiness.ThrowSomeException();

            return Ok("Generic result");
        }
    }
}
