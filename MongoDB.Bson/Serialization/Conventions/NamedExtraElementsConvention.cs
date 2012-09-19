using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MongoDB.Bson.Serialization.Conventions
{
    /// <summary>
    /// Finds an extra elements member convention by name that is of type <see cref="BsonDocument" /> or implements <see cref="IDictionary{string,object}"/>.
    /// </summary>
    public class NamedExtraElementsConvention : IAfterMembersBsonClassMapConvention
    {
        private readonly IEnumerable<string> _names;
        private readonly MemberTypes _memberTypes;
        private readonly BindingFlags _bindingFlags;

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedExtraElementsConvention" /> class.
        /// </summary>
        /// <param name="names">The names.</param>
        public NamedExtraElementsConvention(IEnumerable<string> names)
            : this(names, BindingFlags.Instance | BindingFlags.Public)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedExtraElementsConvention" /> class.
        /// </summary>
        /// <param name="names">The names.</param>
        /// <param name="memberTypes">The member types.</param>
        public NamedExtraElementsConvention(IEnumerable<string> names, MemberTypes memberTypes)
            : this(names, memberTypes, BindingFlags.Instance | BindingFlags.Public)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedExtraElementsConvention" /> class.
        /// </summary>
        /// <param name="names">The names.</param>
        /// <param name="bindingFlags">The binding flags.</param>
        public NamedExtraElementsConvention(IEnumerable<string> names, BindingFlags bindingFlags)
            : this(names, MemberTypes.Field | MemberTypes.Property, bindingFlags)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedExtraElementsConvention" /> class.
        /// </summary>
        /// <param name="names">The names.</param>
        /// <param name="memberTypes">The member types.</param>
        /// <param name="bindingFlags">The binding flags.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public NamedExtraElementsConvention(IEnumerable<string> names, MemberTypes memberTypes, BindingFlags bindingFlags)
        {
            if (names == null)
            {
                throw new ArgumentNullException("names");
            }

            _names = names;
            _memberTypes = memberTypes;
            _bindingFlags = bindingFlags | BindingFlags.DeclaredOnly;
        }

        /// <summary>
        /// Gets the name of the convention.
        /// </summary>
        public string Name
        {
            get { return "NamedExtraElements"; }
        }

        /// <summary>
        /// Applies a modification to the class map.
        /// </summary>
        /// <param name="classMap">The class map.</param>
        public void Apply(BsonClassMap classMap)
        {
            foreach (var name in _names)
            {
                var member = classMap.ClassType.GetMember(name, _memberTypes, _bindingFlags).SingleOrDefault();

                if (member != null)
                {
                    Type memberType = null;
                    var fieldInfo = member as FieldInfo;
                    if (fieldInfo != null)
                    {
                        memberType = fieldInfo.FieldType;
                    }
                    else
                    {
                        var propertyInfo = member as PropertyInfo;
                        if (propertyInfo != null)
                        {
                            memberType = propertyInfo.PropertyType;
                        }
                    }

                    if(memberType != null && (memberType == typeof(BsonDocument) || typeof(IDictionary<string, object>).IsAssignableFrom(memberType)))
                    {
                        classMap.MapExtraElementsMember(member);
                        return;
                    }
                }
            }
        }
    }
}