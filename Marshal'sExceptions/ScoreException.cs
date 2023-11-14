using System;

namespace MarshalsExceptions
{
    public class ScoreException : Exception
    {
        public ScoreException() { }

        public ScoreException(string message)
            : base(message)
        {
        }

        public ScoreException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
