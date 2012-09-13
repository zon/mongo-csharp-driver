using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Bson.Serialization.Conventions
{
    /// <summary>
    /// Marker interface for a convention.
    /// </summary>
    public interface IConvention
    {
        /// <summary>
        /// Gets the name of the convention.
        /// </summary>
        string Name { get; }
    }
}