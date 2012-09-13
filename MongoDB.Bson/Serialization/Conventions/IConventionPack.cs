using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Bson.Serialization.Conventions
{
    /// <summary>
    /// A grouping of conventions.
    /// </summary>
    public interface IConventionPack
    {
        /// <summary>
        /// Gets the conventions.
        /// </summary>
        IEnumerable<IConvention> Conventions { get; }
    }
}