using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MongoDB.Bson.Serialization.Conventions
{
    /// <summary>
    /// Finds read and writeable members for mapping.
    /// </summary>
    public class ReadWriteMemberFinderConvention : IBeforeMembersBsonClassMapConvention
    {
        private readonly BindingFlags _bindingFlags;
        private readonly MemberTypes _memberTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadWriteMemberFinderConvention" /> class.
        /// </summary>
        public ReadWriteMemberFinderConvention()
            : this(BindingFlags.Instance | BindingFlags.Public)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadWriteMemberFinderConvention" /> class.
        /// </summary>
        /// <param name="memberTypes">The member types.</param>
        public ReadWriteMemberFinderConvention(MemberTypes memberTypes)
            : this(memberTypes, BindingFlags.Instance | BindingFlags.Public)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadWriteMemberFinderConvention" /> class.
        /// </summary>
        /// <param name="bindingFlags">The binding flags.</param>
        public ReadWriteMemberFinderConvention(BindingFlags bindingFlags)
            : this(MemberTypes.Field | MemberTypes.Property, bindingFlags)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadWriteMemberFinderConvention" /> class.
        /// </summary>
        /// <param name="memberTypes">The member types.</param>
        /// <param name="bindingFlags">The binding flags.</param>
        public ReadWriteMemberFinderConvention(MemberTypes memberTypes, BindingFlags bindingFlags)
        {
            _memberTypes = memberTypes;
            _bindingFlags = bindingFlags | BindingFlags.DeclaredOnly;
        }

        /// <summary>
        /// Gets the name of the convention.
        /// </summary>
        public string Name
        {
            get { return "ReadWriteMemberFinder"; }
        }

        /// <summary>
        /// Applies a modification to the class map.
        /// </summary>
        /// <param name="classMap">The class map.</param>
        public void Apply(BsonClassMap classMap)
        {
            // order is important for backwards compatibility and GetMembers changes the order of finding things.
            // hence, we'll check member types explicitly instead of letting GetMembers handle it.

            if ((_memberTypes & MemberTypes.Field) == MemberTypes.Field)
            {
                var fields = classMap.ClassType.GetFields(_bindingFlags);
                foreach (var field in fields)
                {
                    MapField(classMap, field);
                }
            }

            if ((_memberTypes & MemberTypes.Property) == MemberTypes.Property)
            {
                var properties = classMap.ClassType.GetProperties(_bindingFlags);
                foreach (var property in properties)
                {
                    MapProperty(classMap, property);
                }
            }
        }

        private void MapField(BsonClassMap classMap, FieldInfo fieldInfo)
        {
            if (fieldInfo.IsInitOnly || fieldInfo.IsLiteral)
            {
                // we can't write
                return;
            }

            classMap.MapMember(fieldInfo);
        }

        private void MapProperty(BsonClassMap classMap, PropertyInfo propertyInfo)
        {
            if (!propertyInfo.CanRead || (!propertyInfo.CanWrite && classMap.ClassType.Namespace != null))
            {
                // we can't write or it is anonymous...
                return;
            }

            // skip indexers
            if (propertyInfo.GetIndexParameters().Length != 0)
            {
                return;
            }

            // skip overridden properties (they are already included by the base class)
            var getMethodInfo = propertyInfo.GetGetMethod(true);
            if (getMethodInfo.IsVirtual && getMethodInfo.GetBaseDefinition().DeclaringType != classMap.ClassType)
            {
                return;
            }

            classMap.MapMember(propertyInfo);
        }
    }
}