using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Bson.Serialization.Conventions
{
    /// <summary>
    /// Convention that applies to a BsonMemberMap.
    /// </summary>
    public interface IBsonMemberMapConvention : IConvention, IBsonMemberMapModifier
    { }
}
