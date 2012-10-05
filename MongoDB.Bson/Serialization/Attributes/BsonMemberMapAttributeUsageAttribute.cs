using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Bson.Serialization
{
    /// <summary>
    /// Indicates the usage restrictions for the attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class BsonMemberMapAttributeUsageAttribute : Attribute
    {
        private bool _allowMultipleMembers;

        /// <summary>
        /// Initializes a new instance of the <see cref="BsonMemberMapAttributeUsageAttribute" /> class.
        /// </summary>
        public BsonMemberMapAttributeUsageAttribute()
        {
            _allowMultipleMembers = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the attribute this attribute applies to is allowed to be applied
        /// to more than one member.
        /// </summary>
        public bool AllowMultipleMembers
        {
            get { return _allowMultipleMembers; }
            set { _allowMultipleMembers = value; }
        }
    }
}