using System;

namespace LWTML
{
    public class LoaderException : Exception
    {
        public LoaderException()
        {
        }

        public LoaderException(string message) : base(message)
        {
        }

        public LoaderException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}