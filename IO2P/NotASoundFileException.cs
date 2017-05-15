using System;
using System.Runtime.Serialization;

namespace IO2P
{

    /// <summary>
    /// Wyjątek rzucany w sytuacji, gdy plik z zadeklarowanym typem dźwięk nie posiada odpowiadającem temu typowi pliku rozszerzenia.
    /// </summary>
    [Serializable]
    internal class NotASoundFileException : Exception
    {
        /*
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
        }*/
    }
}