using System;

namespace Business.ExceptionFilter
{
    public class MyFilterException : Exception
    {
        public MyFilterException(string message) : base(message)
        {
        }

        public MyFilterException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
