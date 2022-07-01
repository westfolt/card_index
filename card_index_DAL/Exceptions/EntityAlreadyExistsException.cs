using System;
using System.Runtime.Serialization;

namespace card_index_DAL.Exceptions
{
    /// <summary>
    /// Exception, thrown when such entity already exists
    /// </summary>
    [Serializable]
    public class EntityAlreadyExistsException : Exception
    {
        /// <summary>
        /// Constructor, takes user message about error as parameter
        /// </summary>
        /// <param name="message">Error describing message</param>
        public EntityAlreadyExistsException(string message) : base(message)
        { }
        /// <summary>
        /// For serialization purposes
        /// </summary>
        /// <param name="info">Data needed to serialize object</param>
        /// <param name="context">Source and destination of given stream</param>
        protected EntityAlreadyExistsException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
