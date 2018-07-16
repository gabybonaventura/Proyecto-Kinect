using System;
using System.Runtime.Serialization;


namespace EmguCV_ColorTracking
{
    [Serializable]
    internal class ColsaNotImplementedException : Exception
    {
        public ColsaNotImplementedException()
        {
        }

        public ColsaNotImplementedException(string message) : base(message)
        {
        }

        public ColsaNotImplementedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ColsaNotImplementedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
