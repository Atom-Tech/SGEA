using System;
using System.Runtime.Serialization;

namespace Apresentacao.Slide
{
    [Serializable]
    public class AnimationException : Exception
    {
        public AnimationException()
        {
        }

        public AnimationException(string message) : base(message)
        {
        }

        public AnimationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AnimationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}