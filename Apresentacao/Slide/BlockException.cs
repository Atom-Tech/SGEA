using System;
using System.Runtime.Serialization;

namespace Apresentacao.Slide
{
    [Serializable]
    internal class BlockException : Exception
    {
        public BlockException()
        {
        }

        public BlockException(string message) : base(message)
        {
        }

        public BlockException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BlockException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}