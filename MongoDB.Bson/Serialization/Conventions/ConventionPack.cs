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
        /// Adds a convention for a BsonClassMap to run after the members have been mapped.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="action">The action.</param>
        public void AddAfterMembersBsonClassMapConvention(string name, Action<BsonClassMap> action)
        {
            Add(new DelegateAfterMembersBsonClassMapConvention(name, action));
        }

        /// <summary>
        /// Adds a convention for a BsonClassMap to run before the members have been mapped.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="action">The action.</param>
        public void AddBeforeMembersBsonClassMapConvention(string name, Action<BsonClassMap> action)
        {
            Add(new DelegateBeforeMembersBsonClassMapConvention(name, action));
        }

        /// <summary>
        /// Adds a convention for a BsonMemberMap.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="action">The action.</param>
        public void AddBsonMemberMapConvention(string name, Action<BsonMemberMap> action)
        {
            Add(new DelegateBsonMemberMapConvention(name, action));
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
    }
}