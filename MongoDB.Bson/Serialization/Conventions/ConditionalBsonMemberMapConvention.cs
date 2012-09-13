using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Bson.Serialization.Conventions
{
    /// <summary>
    /// Convention that applies to a BsonMemberMap conditionally.
    /// </summary>
    public abstract class ConditionalBsonMemberMapConvention : IBsonMemberMapConvention
    {
        /// <summary>
        /// Gets the name of the convention.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Indicates whether the specified memberMap is able to apply the convention.
        /// </summary>
        /// <param name="memberMap">The member map.</param>
        /// <returns>true if the convention is applicable; otherwise false.</returns>
        protected abstract bool Matches(BsonMemberMap memberMap);

        /// <summary>
        /// Applies a modification to the member map.
        /// </summary>
        /// <param name="memberMap">The member map.</param>
        void IBsonMemberMapModifier.Apply(BsonMemberMap memberMap)
        {
            if (!Matches(memberMap))
            {
                return;
            }

            Apply(memberMap);
        }

        /// <summary>
        /// Applies a modification to the member map.
        /// </summary>
        /// <param name="memberMap">The member map.</param>
        protected abstract void Apply(BsonMemberMap memberMap);
    }
}
