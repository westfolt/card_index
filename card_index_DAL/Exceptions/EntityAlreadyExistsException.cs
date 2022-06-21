using System;
using System.Runtime.Serialization;

namespace card_index_DAL.Exceptions
{
    [Serializable]
    public class EntityAlreadyExistsException : Exception
    {
        public EntityAlreadyExistsException(string message) : base(message)
        { }

        protected EntityAlreadyExistsException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
