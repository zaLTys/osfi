using System;

namespace StatistinesAtaskaitos.Services
{
    public class WTFException : Exception
    {
        public WTFException(string message) : base(message)
        {
        }
    }
}