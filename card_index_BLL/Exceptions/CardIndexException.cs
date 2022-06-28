using System;
using System.Runtime.Serialization;

namespace card_index_BLL.Exceptions
{
    /// <summary>
    /// Custom exception for BLL errors
    /// </summary>
    [Serializable]
    public class CardIndexException : Exception
    {
        /// <summary>
        /// Takes error message as parameter
        /// </summary>
        /// <param name="message">Error message</param>
        public CardIndexException(string message) : base(message)
        { }
        /// <summary>
        /// Takes error message and inner exception, caused current
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="innerException">Inner exception</param>
        public CardIndexException(string message, Exception innerException) : base(message, innerException)
        { }
        /// <summary>
        /// For serialization purposes
        /// </summary>
        /// <param name="info">Stores data for obj serialization</param>
        /// <param name="context">Context of serialization stream</param>
        protected CardIndexException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
