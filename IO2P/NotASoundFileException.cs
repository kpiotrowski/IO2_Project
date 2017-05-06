using System;
using System.Runtime.Serialization;

namespace IO2P
{
    [Serializable]
    internal class NotASoundFileException : Exception
    {
        public NotASoundFileException()
        {
        }

        public NotASoundFileException(string message) : base(message)
        {
        }

        public NotASoundFileException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotASoundFileException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}