using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Bson.Serialization.Conventions
{
    /// <summary>
    /// Applies the BsonClassMap modification conditionally.
    /// </summary>
    public abstract class ConditionalAfterMembersBsonClassMapConvention : IAfterMembersBsonClassMapConvention
    {
        /// <summary>
        /// Gets the name of the convention.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Indicates whether the specified classMap is able to apply the convention.
        /// </summary>
        /// <param name="classMap">The class map.</param>
        /// <returns>true if the convention is applicable; otherwise false.</returns>
        protected abstract bool Matches(BsonClassMap classMap);

        /// <summary>
        /// Applies a modification to the class map.
        /// </summary>
        /// <param name="classMap">The class map.</param>
        void IAfterMembersBsonClassMapConvention.Apply(BsonClassMap classMap)
        {
            if (!Matches(classMap))
            {
                return;
            }

            Apply(classMap);
        }

        /// <summary>
        /// Applies a modification to the class map.
        /// </summary>
        /// <param name="classMap">The class map.</param>
        protected abstract void Apply(BsonClassMap classMap);
    }
}
