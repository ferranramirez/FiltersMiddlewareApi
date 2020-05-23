using Business.ExceptionFilter;
using Microsoft.Extensions.Logging;
using System;

namespace Business
{
    public class FiltersBusiness
    {
        private readonly ILogger<FiltersBusiness> _logger;

        public FiltersBusiness(ILogger<FiltersBusiness> logger)
        {
            _logger = logger;
        }

        public void ParentThrowSomeException()
        {
            try
            {
                _logger.LogDebug("Business class: ParentThrowSomeException()");
                ThrowSomeException();
            }
            catch (ArithmeticException ex)
            {
                throw new MyFilterException("This is my really handled custom exception", ex, 1234);
            }
        }

        public void ThrowSomeException()
        {
            _logger.LogDebug("Business class: ThrowSomeException()");
            throw new ArithmeticException("This is a super random exception");
        }

    }
}
