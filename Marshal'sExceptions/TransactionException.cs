using System;

namespace MarshalsExceptions
{
    public class TransactionException : Exception
    {
        public TransactionException() { }

        public TransactionException(string message)
            : base(message)
        {
        }

        public TransactionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
