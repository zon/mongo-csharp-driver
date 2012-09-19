using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Bson.Serialization.Conventions
{
    /// <summary>
    /// Convention to set whether a class map should ignore extra elements.
    /// </summary>
    public class IgnoreExtraElementsConvention : IBeforeMembersBsonClassMapConvention
    {
        private readonly bool _ignoreExtraElements;

        /// <summary>
        /// Initializes a new instance of the <see cref="IgnoreExtraElementsConvention" /> class.
        /// </summary>
        /// <param name="ignoreExtraElements">if set to <c>true</c> [ignore extra elements].</param>
        public IgnoreExtraElementsConvention(bool ignoreExtraElements)
        {
            _ignoreExtraElements = ignoreExtraElements;
        }

        /// <summary>
        /// Gets the name of the convention.
        /// </summary>
        public string Name
        {
            get { return "IgnoreExtraElements"; }
        }

        /// <summary>
        /// Applies a modification to the class map.
        /// </summary>
        /// <param name="classMap">The class map.</param>
        public void Apply(BsonClassMap classMap)
        {
            classMap.SetIgnoreExtraElements(_ignoreExtraElements);
        }
    }
}