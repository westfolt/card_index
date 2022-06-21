using System;
using System.Runtime.Serialization;

namespace card_index_DAL.Exceptions
{
    [Serializable]
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string message) : base(message)
        { }

        protected EntityNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
