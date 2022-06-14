using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

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
