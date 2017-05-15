using System;
using System.Runtime.Serialization;

namespace IO2P
{
    /// <summary>
    /// Wyjątek rzucany w sytuacji, gdy plik z zadeklarowanym typem wideo nie posiada odpowiadającem temu typowi pliku rozszerzenia.
    /// </summary>
    [Serializable]
    internal class NotAVideoFileException : Exception
    {
        /*
        public NotAVideoFileException()
        {
        }

        public NotAVideoFileException(string message) : base(message)
        {
        }

        public NotAVideoFileException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotAVideoFileException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }*/
    }
}