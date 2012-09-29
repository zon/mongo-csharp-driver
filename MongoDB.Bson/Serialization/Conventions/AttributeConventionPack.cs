/* Copyright 2010-2012 10gen Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

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
        // private static fields
        private static readonly AttributeConventionPack __attributeConventionPack = new AttributeConventionPack();

        // private fields
        private readonly AttributeConvention _attributeConvention;

        // constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeConventionPack" /> class.
        /// </summary>
        private AttributeConventionPack()
        {
            _attributeConvention = new AttributeConvention();
        }

        // public static properties
        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static IConventionPack Instance
        {
            get { return __attributeConventionPack; }
        }

        // public properties
        /// <summary>
        /// Gets the conventions.
        /// </summary>
        public IEnumerable<IConvention> Conventions
        {
            get { yield return _attributeConvention; }
        }

        // nested classes
        private class AttributeConvention : ConventionBase, IClassMapConvention, IMemberMapConvention
        {
            // public methods
            public void Apply(BsonClassMap classMap)
            {
                foreach (IBsonClassMapModifier attribute in classMap.ClassType.GetCustomAttributes(typeof(IBsonClassMapModifier), false))
                {
                    attribute.Apply(classMap);
                }

                OptInMembersWithBsonMemberMapModifierAttribute(classMap);
                IgnoreMembersWithBsonIgnoreAttribute(classMap);
            }

            // public methods
            public void Apply(BsonMemberMap memberMap)
            {
                foreach (IBsonMemberMapModifier attribute in memberMap.MemberInfo.GetCustomAttributes(typeof(IBsonMemberMapModifier), false))
                {
                    attribute.Apply(memberMap);
                }
            }

            // private methods
            private void OptInMembersWithBsonMemberMapModifierAttribute(BsonClassMap classMap)
            {
                // let other fields opt-in if they have any IBsonMemberMapModifier attributes
                foreach (var fieldInfo in classMap.ClassType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
                {
                    foreach (var attribute in fieldInfo.GetCustomAttributes(false))
                    {
                        if (typeof(IBsonMemberMapModifier).IsAssignableFrom(attribute.GetType()))
                        {
                            var memberMap = classMap.GetMemberMap(fieldInfo.Name);
                            if (memberMap == null)
                            {
                                classMap.MapMember(fieldInfo);
                            }
                        }
                    }
                }

                // let other properties opt-in if they have any IBsonMemberMapModifier attributes
                foreach (var propertyInfo in classMap.ClassType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
                {
                    foreach (var attribute in propertyInfo.GetCustomAttributes(false))
                    {
                        if (typeof(IBsonMemberMapModifier).IsAssignableFrom(attribute.GetType()))
                        {
                            var memberMap = classMap.GetMemberMap(propertyInfo.Name);
                            if (memberMap == null)
                            {
                                classMap.MapMember(propertyInfo);
                            }
                        }
                    }
                }
            }

            private void IgnoreMembersWithBsonIgnoreAttribute(BsonClassMap classMap)
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
