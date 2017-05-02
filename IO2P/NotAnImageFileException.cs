using System;
using System.Runtime.Serialization;

namespace IO2P
{
    [Serializable]
    internal class NotAnImageFileException : Exception
    {
        public NotAnImageFileException()
        {
        }

        public NotAnImageFileException(string message) : base(message)
        {
        }

        public NotAnImageFileException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotAnImageFileException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}