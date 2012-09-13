using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MongoDB.Bson.Serialization.Conventions
{
    /// <summary>
    /// Convention pack for applying attributes.
    /// </summary>
    public class AttributeConventionPack : IConventionPack
    {
        private static readonly AttributeConventionPack __attributeConventionPack = new AttributeConventionPack();
        private readonly AttributeConvention _attributeConvention;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static IConventionPack Instance
        {
            get { return __attributeConventionPack; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeConventionPack" /> class.
        /// </summary>
        private AttributeConventionPack()
        {
            _attributeConvention = new AttributeConvention();
        }

        /// <summary>
        /// Gets the conventions.
        /// </summary>
        public IEnumerable<IConvention> Conventions
        {
            get { yield return _attributeConvention; }
        }

        private class AttributeConvention : IBeforeMembersBsonClassMapConvention, IBsonMemberMapConvention
        {
            public string Name
            {
                get { return "Attributes"; }
            }

            public void Apply(BsonClassMap classMap)
            {
                foreach (IBsonClassMapModifier attribute in classMap.ClassType.GetCustomAttributes(typeof(IBsonClassMapModifier), false))
                {
                    attribute.Apply(classMap);
                }

                OptInMembersWithBsonElementAttribute(classMap);
                IgnoreMembersWithBsonIgnoreAttribute(classMap);
            }

            public void Apply(BsonMemberMap memberMap)
            {
                foreach (IBsonMemberMapModifier attribute in memberMap.MemberInfo.GetCustomAttributes(typeof(IBsonMemberMapModifier), false))
                {
                    attribute.Apply(memberMap);
                }
            }

            private static void OptInMembersWithBsonElementAttribute(BsonClassMap classMap)
            {
                // use a List instead of a HashSet to preserver order
                var memberInfos = new List<MemberInfo>();

                // let other fields opt-in if they have a BsonElement attribute
                foreach (var fieldInfo in classMap.ClassType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
                {
                    var elementAttribute = (BsonElementAttribute)fieldInfo.GetCustomAttributes(typeof(BsonElementAttribute), false).FirstOrDefault();
                    if (elementAttribute == null)
                    {
                        continue;
                    }

                    if (!memberInfos.Contains(fieldInfo))
                    {
                        classMap.MapMember(fieldInfo);
                    }
                }

                // let other properties opt-in if they have a BsonElement attribute
                foreach (var propertyInfo in classMap.ClassType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
                {
                    var elementAttribute = (BsonElementAttribute)propertyInfo.GetCustomAttributes(typeof(BsonElementAttribute), false).FirstOrDefault();
                    if (elementAttribute == null)
                    {
                        continue;
                    }

                    if (!memberInfos.Contains(propertyInfo))
                    {
                        classMap.MapMember(propertyInfo);
                    }
                }
            }

            private static void IgnoreMembersWithBsonIgnoreAttribute(BsonClassMap classMap)
            {
                foreach (var memberMap in classMap.DeclaredMemberMaps.ToList())
                {
                    var ignoreAttribute = (BsonIgnoreAttribute)memberMap.MemberInfo.GetCustomAttributes(typeof(BsonIgnoreAttribute), false).FirstOrDefault();
                    if (ignoreAttribute != null)
                    {
                        classMap.UnmapMember(memberMap.MemberInfo);
                    }
                }
            }
        }
    }
}