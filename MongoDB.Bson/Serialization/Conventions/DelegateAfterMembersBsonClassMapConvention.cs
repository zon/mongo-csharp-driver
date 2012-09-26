using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Bson.Serialization.Conventions
{
    /// <summary>
    /// A BsonClassMap convention that wraps a delegate.
    /// </summary>
    public class DelegateAfterMembersBsonClassMapConvention : IAfterMembersBsonClassMapConvention
    {
        private readonly string _name;
        private readonly Action<BsonClassMap> _action;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateAfterMembersBsonClassMapConvention" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="action">The action.</param>
        public DelegateAfterMembersBsonClassMapConvention(string name, Action<BsonClassMap> action)
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
        /// Applies a modification to the class map.
        /// </summary>
        /// <param name="classMap">The class map.</param>
        public void Apply(BsonClassMap classMap)
        {
            _action(classMap);
        }
    }
}