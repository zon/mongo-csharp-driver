using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Bson.Serialization.Conventions
{
    /// <summary>
    /// Base class for a convention that sets the name to the Type's name without the convention suffix.
    /// </summary>
    public abstract class ConventionBase : IConvention
    {
        /// <summary>
        /// Gets the name of the convention.
        /// </summary>
        public string Name
        {
            get { return GetName(); }
        }

        /// <summary>
        /// Gets the name of the convention.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetName()
        {
            var name = this.GetType().Name;
            if (name.EndsWith("Convention"))
            {
                return name.Substring(0, name.Length - 10);
            }

            return name;
        }
    }
}