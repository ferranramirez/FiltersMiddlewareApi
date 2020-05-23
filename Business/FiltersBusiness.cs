using Business.ExceptionFilter;
using System;

namespace Business
{
    public class FiltersBusiness
    {
        public void ParentThrowSomeException()
        {
            try
            {
                ThrowSomeException();
            }
            catch (ArithmeticException ex)
            {
                throw new MyFilterException("This is my really handled custom exception", ex, 1234);
            }
        }

        public void ThrowSomeException()
        {
            throw new ArithmeticException("This is a super random exception");
        }

    }
}
