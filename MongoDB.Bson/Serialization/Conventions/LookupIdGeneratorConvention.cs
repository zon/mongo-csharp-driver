using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Bson.Serialization.Conventions
{
    /// <summary>
    /// Convention to lookup an id generator in the global id generator registrations.
    /// </summary>
    public class LookupIdGeneratorConvention : ConventionBase, IAfterMembersBsonClassMapConvention
    {
        /// <summary>
        /// Applies a modification to the class map.
        /// </summary>
        /// <param name="classMap">The class map.</param>
        public void Apply(BsonClassMap classMap)
        {
            var idMemberMap = classMap.IdMemberMap;
            if (idMemberMap != null)
            {
                if (idMemberMap.IdGenerator != null)
                {
                    return;
                }

                idMemberMap.SetIdGenerator(BsonSerializer.LookupIdGenerator(idMemberMap.MemberType));
            }
        }
    }
}
