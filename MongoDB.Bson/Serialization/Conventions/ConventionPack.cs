using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Bson.Serialization.Conventions
{
    /// <summary>
    /// A mutable pack of conventions.
    /// </summary>
    public class ConventionPack : IConventionPack
    {
        private readonly List<IConvention> _conventions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConventionPack" /> class.
        /// </summary>
        public ConventionPack()
        {
            _conventions = new List<IConvention>();
        }

        /// <summary>
        /// Gets the conventions.
        /// </summary>
        public IEnumerable<IConvention> Conventions
        {
            get { return _conventions; }
        }

        /// <summary>
        /// Adds the specified convention.
        /// </summary>
        /// <param name="convention">The convention.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public void Add(IConvention convention)
        {
            if (convention == null)
            {
                throw new ArgumentNullException("convention");
            }

            _conventions.Add(convention);
        }

        /// <summary>
        /// Adds the range of conventions.
        /// </summary>
        /// <param name="conventions">The conventions.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public void AddRange(IEnumerable<IConvention> conventions)
        {
            if (conventions == null)
            {
                throw new ArgumentNullException("conventions");
            }

            _conventions.AddRange(conventions);
        }

        /// <summary>
        /// Appends the conventions in other to the end of this pack.
        /// </summary>
        /// <param name="other">The other.</param>
        public void Append(IConventionPack other)
        {
            AddRange(other.Conventions);
        }

        /// <summary>
        /// Inserts the convention after another convention specified by the name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="convention">The convention.</param>
        public void InsertAfter(string name, IConvention convention)
        {
            var index = _conventions.FindIndex(x => x.Name == name) + 1;
            if (index < 1)
            {
                var message = string.Format("Unable to find a convention by the name of '{0}'.", name);
                throw new ArgumentOutOfRangeException("name", message);
            }

            _conventions.Insert(index, convention);
        }

        /// <summary>
        /// Inserts the convention before another convention specified by the name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="convention">The convention.</param>
        public void InsertBefore(string name, IConvention convention)
        {
            var index = _conventions.FindIndex(x => x.Name == name);
            if (index < 0)
            {
                var message = string.Format("Unable to find a convention by the name of '{0}'.", name);
                throw new ArgumentOutOfRangeException("name", message);
            }

            _conventions.Insert(index, convention);
        }

        /// <summary>
        /// Removes the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        public void Remove(string name)
        {
            _conventions.RemoveAll(x => x.Name == name);
        }

        /// <summary>
        /// Creates a convention pack from a legacy ConventionProfile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns></returns>
        [Obsolete("This is a helper method and will be removed when the old conventions are removed.")]
        public static IConventionPack FromConventionProfile(ConventionProfile profile)
        {
            var pack = new ConventionPack();

            // class mapping conventions
            if (profile.MemberFinderConvention != null)
            {
                pack.Add(new MemberFinderConventionWrapper(profile.MemberFinderConvention));
            }
            if (profile.IdMemberConvention != null)
            {
                pack.Add(new IdMemberConventionWrapper(profile.IdMemberConvention));
            }
            if (profile.IdGeneratorConvention != null)
            {
                pack.Add(new IdGeneratorConventionWrapper(profile.IdGeneratorConvention));
            }
            if (profile.ExtraElementsMemberConvention != null)
            {
                pack.Add(new ExtraElementsConventionWrapper(profile.ExtraElementsMemberConvention));
            }
            if (profile.IgnoreExtraElementsConvention != null)
            {
                pack.Add(new IgnoreExtraElementsConventionWrapper(profile.IgnoreExtraElementsConvention));
            }

            // member mapping conventions
            if (profile.DefaultValueConvention != null)
            {
                pack.Add(new DefaultValueConventionWrapper(profile.DefaultValueConvention));
            }
            if (profile.IgnoreIfDefaultConvention != null)
            {
                pack.Add(new IgnoreIfDefaultConventionWrapper(profile.IgnoreIfDefaultConvention));
            }
            if (profile.IgnoreIfNullConvention != null)
            {
                pack.Add(new IgnoreIfNullConventionWrapper(profile.IgnoreIfNullConvention));
            }
            if (profile.SerializationOptionsConvention != null)
            {
                pack.Add(new SerializationOptionsConventionWrapper(profile.SerializationOptionsConvention));
            }
            if (profile.ElementNameConvention != null)
            {
                pack.Add(new ElementNameConventionWrapper(profile.ElementNameConvention));
            }
            return pack;
        }
    }
}
