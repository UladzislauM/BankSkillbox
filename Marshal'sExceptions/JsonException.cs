﻿using System;

namespace MarshalsExceptions
{
    public class JsonException : Exception
    {
        public JsonException() { }

        public JsonException(string message)
            : base(message)
        {
        }

        public JsonException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
