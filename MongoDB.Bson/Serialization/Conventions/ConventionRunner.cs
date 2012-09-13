using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Bson.Serialization.Conventions
{
    /// <summary>
    /// Runs the conventions against a BsonClassMap and its BsonMemberMaps.
    /// </summary>
    public class ConventionRunner : IBsonClassMapModifier
    {
        private readonly IEnumerable<IConvention> _conventions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConventionRunner" /> class.
        /// </summary>
        /// <param name="conventions">The conventions.</param>
        public ConventionRunner(IConventionPack conventions)
        {
            if (conventions == null)
            {
                throw new ArgumentNullException("conventions");
            }

            _conventions = conventions.Conventions.ToList();
        }

        /// <summary>
        /// Applies a modification to the class map.
        /// </summary>
        /// <param name="classMap">The class map.</param>
        public void Apply(BsonClassMap classMap)
        {
            foreach (var convention in _conventions.OfType<IBeforeMembersBsonClassMapConvention>())
            {
                convention.Apply(classMap);
            }

            foreach (var convention in _conventions.OfType<IBsonMemberMapConvention>())
            {
                foreach (var memberMap in classMap.DeclaredMemberMaps)
                {
                    convention.Apply(memberMap);
                }
            }

            foreach (var convention in _conventions.OfType<IAfterMembersBsonClassMapConvention>())
            {
                convention.Apply(classMap);
            }
        }
    }
}