using System;

namespace MarshalsExceptions
{
    public class DBException : Exception
    {
        public DBException() { }

        public DBException(string message)
            : base(message)
        {
        }

        public DBException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
