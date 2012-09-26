using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Bson.Serialization.Conventions
{
    /// <summary>
    /// A BsonMemberMap convention that wraps a delegate.
    /// </summary>
    public class DelegateBsonMemberMapConvention : IBsonMemberMapConvention
    {
        private readonly string _name;
        private readonly Action<BsonMemberMap> _action;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateBsonMemberMapConvention" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="action">The action.</param>
        public DelegateBsonMemberMapConvention(string name, Action<BsonMemberMap> action)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            _name = name;
            _action = action;
        }

        /// <summary>
        /// Gets the name of the convention.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Applies a modification to the member map.
        /// </summary>
        /// <param name="memberMap">The member map.</param>
        public void Apply(BsonMemberMap memberMap)
        {
            _action(memberMap);
        }
    }
}