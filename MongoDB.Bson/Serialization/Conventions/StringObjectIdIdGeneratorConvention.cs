using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Bson.Serialization.Conventions
{
    /// <summary>
    /// Convention for setting an id generator for a string member with a bson representation of ObjectId.
    /// </summary>
    public class StringObjectIdIdGeneratorConvention : IAfterMembersBsonClassMapConvention
    {
        /// <summary>
        /// Gets the name of the convention.
        /// </summary>
        public string Name
        {
            get { return "StringObjectIdIdGenerator"; }
        }

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

                var representationOptions = idMemberMap.SerializationOptions as RepresentationSerializationOptions;
                if (idMemberMap.MemberType == typeof(string) && representationOptions != null && representationOptions.Representation == BsonType.ObjectId)
                {
                    idMemberMap.SetIdGenerator(StringObjectIdGenerator.Instance);
                }
            }
        }
    }
}