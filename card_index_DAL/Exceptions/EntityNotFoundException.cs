using System;
using System.Runtime.Serialization;

namespace card_index_DAL.Exceptions
{
    /// <summary>
    /// Exception, thrown when entity cannot be found
    /// </summary>
    [Serializable]
    public class EntityNotFoundException : Exception
    {
        /// <summary>
        /// Constructor, takes user message about error as parameter
        /// </summary>
        /// <param name="message">Error describing message</param>
        public EntityNotFoundException(string message) : base(message)
        { }
        /// <summary>
        /// For serialization purposes
        /// </summary>
        /// <param name="info">Data needed to serialize object</param>
        /// <param name="context">Source and destination of given stream</param>
        protected EntityNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
