using System;

namespace LangAppApi.Domain.Exceptions
{
    public class AuthException : Exception
    {
        public AuthException()
        {
        }

        public AuthException(string message) : base(message)
        {
        }

        public AuthException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}