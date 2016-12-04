using System;
using System.Runtime.Serialization;

namespace Apresentacao.Slide
{
    [Serializable]
    public class SlideException : Exception
    {
        public SlideException()
        {
        }

        public SlideException(string message) : base(message)
        {
        }

        public SlideException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SlideException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}