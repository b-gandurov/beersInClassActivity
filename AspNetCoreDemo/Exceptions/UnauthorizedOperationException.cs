using System;

namespace AspNetCoreDemo.Exceptions
{
    public class UnauthorizedOperationException : Exception
    {
        public UnauthorizedOperationException()
            : base()
        {
        }

        public UnauthorizedOperationException(string message)
            : base(message)
        {
        }
    }
}
