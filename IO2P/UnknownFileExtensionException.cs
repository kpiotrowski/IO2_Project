using System;
using System.Runtime.Serialization;

namespace IO2P
{
    /// <summary>
    /// Wyjątek rzucany w sytuacji, gdy do przetwarzania został podany plik pozbawiony rozszerzania.
    /// </summary>
    [Serializable]
    internal class UnknownFileExtensionException : Exception
    {
        /*
        public UnknownFileExtensionException()
        {
        }

        public UnknownFileExtensionException(string message) : base(message)
        {
        }

        public UnknownFileExtensionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnknownFileExtensionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }*/
    }
}