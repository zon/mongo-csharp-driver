using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MongoDB.Bson.Serialization.Conventions
{
    internal class DefaultValueConventionWrapper : IBsonMemberMapConvention
    {
        private readonly IDefaultValueConvention _convention;

        public DefaultValueConventionWrapper(IDefaultValueConvention convention)
        {
            if (convention == null)
            {
                throw new ArgumentNullException("convention");
            }

            _convention = convention;
        }

        public string Name
        {
            get { return _convention.GetType().Name; }
        }

        public void Apply(BsonMemberMap memberMap)
        {
            var value = _convention.GetDefaultValue(memberMap.MemberInfo);
            if (value != null)
            {
                memberMap.SetDefaultValue(value);
            }
        }
    }

    internal class ElementNameConventionWrapper : IBsonMemberMapConvention
    {
        private readonly IElementNameConvention _convention;

        public ElementNameConventionWrapper(IElementNameConvention convention)
        {
            if (convention == null)
            {
                throw new ArgumentNullException("convention");
            }

            _convention = convention;
        }

        public string Name
        {
            get { return _convention.GetType().Name; }
        }

        public void Apply(BsonMemberMap memberMap)
        {
            var name = _convention.GetElementName(memberMap.MemberInfo);
            memberMap.SetElementName(name);
        }
    }

    internal class ExtraElementsConventionWrapper : IAfterMembersBsonClassMapConvention
    {
        private readonly IExtraElementsMemberConvention _convention;

        public ExtraElementsConventionWrapper(IExtraElementsMemberConvention convention)
        {
            if (convention == null)
            {
                throw new ArgumentNullException("convention");
            }

            _convention = convention;
        }

        public string Name
        {
            get { return _convention.GetType().Name; }
        }

        public void Apply(BsonClassMap classMap)
        {
            var memberName = _convention.FindExtraElementsMember(classMap.ClassType);
            if (string.IsNullOrEmpty(memberName))
            {
                return;
            }

            var memberInfo = classMap.ClassType.GetMember(memberName, MemberTypes.Field | MemberTypes.Property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).SingleOrDefault();
            if (memberInfo == null)
            {
                return;
            }

            classMap.MapExtraElementsMember(memberInfo);
        }
    }

    internal class IdGeneratorConventionWrapper : IAfterMembersBsonClassMapConvention
    {
        private readonly IIdGeneratorConvention _convention;

        public IdGeneratorConventionWrapper(IIdGeneratorConvention convention)
        {
            if (convention == null)
            {
                throw new ArgumentNullException("convention");
            }

            _convention = convention;
        }

        public string Name
        {
            get { return _convention.GetType().Name; }
        }

        public void Apply(BsonClassMap classMap)
        {
            var idMemberMap = classMap.IdMemberMap;
            if (idMemberMap == null)
            {
                return;
            }

            var representationOptions = idMemberMap.SerializationOptions as RepresentationSerializationOptions;
            if (idMemberMap.MemberType == typeof(string) && representationOptions != null && representationOptions.Representation == BsonType.ObjectId)
            {
                idMemberMap.SetIdGenerator(StringObjectIdGenerator.Instance);
            }
            else
            {
                var generator = _convention.GetIdGenerator(classMap.IdMemberMap.MemberInfo);
                idMemberMap.SetIdGenerator(generator);
            }
        }
    }

    internal class IdMemberConventionWrapper : IBeforeMembersBsonClassMapConvention
    {
        private readonly IIdMemberConvention _convention;

        public IdMemberConventionWrapper(IIdMemberConvention convention)
        {
            if (convention == null)
            {
                throw new ArgumentNullException("convention");
            }

            _convention = convention;
        }

        public string Name
        {
            get { return _convention.GetType().Name; }
        }

        public void Apply(BsonClassMap classMap)
        {
            var memberName = _convention.FindIdMember(classMap.ClassType);
            if (string.IsNullOrEmpty(memberName))
            {
                return;
            }

            var memberInfo = classMap.ClassType.GetMember(memberName, MemberTypes.Field | MemberTypes.Property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).SingleOrDefault();
            if (memberInfo == null)
            {
                return;
            }

            classMap.SetIdMember(classMap.MapMember(memberInfo));
        }
    }

    internal class IgnoreExtraElementsConventionWrapper : IBeforeMembersBsonClassMapConvention
    {
        private readonly IIgnoreExtraElementsConvention _convention;

        public IgnoreExtraElementsConventionWrapper(IIgnoreExtraElementsConvention convention)
        {
            if (convention == null)
            {
                throw new ArgumentNullException("convention");
            }

            _convention = convention;
        }

        public string Name
        {
            get { return _convention.GetType().Name; }
        }

        public void Apply(BsonClassMap classMap)
        {
            var value = _convention.IgnoreExtraElements(classMap.ClassType);
            classMap.SetIgnoreExtraElements(value);
        }
    }

    internal class IgnoreIfDefaultConventionWrapper : IBsonMemberMapConvention
    {
        private readonly IIgnoreIfDefaultConvention _convention;

        public IgnoreIfDefaultConventionWrapper(IIgnoreIfDefaultConvention convention)
        {
            if (convention == null)
            {
                throw new ArgumentNullException("convention");
            }

            _convention = convention;
        }

        public string Name
        {
            get { return _convention.GetType().Name; }
        }

        public void Apply(BsonMemberMap memberMap)
        {
            var value = _convention.IgnoreIfDefault(memberMap.MemberInfo);
            memberMap.SetIgnoreIfDefault(value);
        }
    }

    internal class IgnoreIfNullConventionWrapper : IBsonMemberMapConvention
    {
        private readonly IIgnoreIfNullConvention _convention;

        public IgnoreIfNullConventionWrapper(IIgnoreIfNullConvention convention)
        {
            if (convention == null)
            {
                throw new ArgumentNullException("convention");
            }

            _convention = convention;
        }

        public string Name
        {
            get { return _convention.GetType().Name; }
        }

        public void Apply(BsonMemberMap memberMap)
        {
            var value = _convention.IgnoreIfNull(memberMap.MemberInfo);
            memberMap.SetIgnoreIfNull(value);
        }
    }

    internal class MemberFinderConventionWrapper : IBeforeMembersBsonClassMapConvention
    {
        private readonly IMemberFinderConvention _convention;
        private readonly bool _removeOldMemberMaps;

        public MemberFinderConventionWrapper(IMemberFinderConvention convention)
            : this(convention, true)
        { }

        public MemberFinderConventionWrapper(IMemberFinderConvention convention, bool removeOldMemberMaps)
        {
            if (convention == null)
            {
                throw new ArgumentNullException("convention");
            }

            _convention = convention;
            _removeOldMemberMaps = removeOldMemberMaps;
        }

        public string Name
        {
            get { return _convention.GetType().Name; }
        }

        public void Apply(BsonClassMap classMap)
        {
            var members = _convention.FindMembers(classMap.ClassType);
            var alreadyMappedMembers = classMap.DeclaredMemberMaps.Select(x => x.MemberInfo);

            if (_removeOldMemberMaps)
            {
                var unmapMembers = alreadyMappedMembers.Except(members);
                foreach (var member in unmapMembers)
                {
                    classMap.UnmapMember(member);
                }
            }

            var mapMembers = members.Except(alreadyMappedMembers);
            foreach (var member in mapMembers)
            {
                classMap.MapMember(member);
            }
        }
    }

    internal class SerializationOptionsConventionWrapper : IBsonMemberMapConvention
    {
        private readonly ISerializationOptionsConvention _convention;

        public SerializationOptionsConventionWrapper(ISerializationOptionsConvention convention)
        {
            if (convention == null)
            {
                throw new ArgumentNullException("convention");
            }

            _convention = convention;
        }

        public string Name
        {
            get { return _convention.GetType().Name; }
        }

        public void Apply(BsonMemberMap memberMap)
        {
            var value = _convention.GetSerializationOptions(memberMap.MemberInfo);
            memberMap.SetSerializationOptions(value);
        }
    }
}