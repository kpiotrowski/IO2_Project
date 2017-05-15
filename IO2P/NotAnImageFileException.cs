using System;
using System.Runtime.Serialization;

namespace IO2P
{

    /// <summary>
    /// Wyjątek rzucany w sytuacji, gdy plik z zadeklarowanym typem obrazek nie posiada odpowiadającem temu typowi pliku rozszerzenia.
    /// </summary>
    [Serializable]
    internal class NotAnImageFileException : Exception
    {
        /*
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
        }*/
    }
}