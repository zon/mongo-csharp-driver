using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Bson
{
    /// <summary>
    /// Indicates that an attribute restricted to one member has been applied to multiple members.
    /// </summary>
    [Serializable]
    public class DuplicateBsonMemberMapAttributeException : BsonException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateBsonMemberMapAttributeException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public DuplicateBsonMemberMapAttributeException(string message) 
            : base(message) 
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateBsonMemberMapAttributeException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public DuplicateBsonMemberMapAttributeException(string message, Exception inner) 
            : base(message, inner) 
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateBsonMemberMapAttributeException" /> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        protected DuplicateBsonMemberMapAttributeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
