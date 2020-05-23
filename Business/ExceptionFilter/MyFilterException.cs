using System;

namespace Business.ExceptionFilter
{
    public class MyFilterException : Exception
    {
        public int ErrorCode { get; set; }

        public MyFilterException(string message, int errorCode = 500) : base(message)
        {
            ErrorCode = errorCode;
        }

        public MyFilterException(string message,
                                Exception innerException,
                                int errorCode = 500) : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}
