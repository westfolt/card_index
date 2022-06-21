using System;
using System.Runtime.Serialization;

namespace card_index_BLL.Exceptions
{
    [Serializable]
    public class CardIndexException : Exception
    {
        public CardIndexException(string message) : base(message)
        { }
        public CardIndexException(string message, Exception innerException) : base(message, innerException)
        { }

        protected CardIndexException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
