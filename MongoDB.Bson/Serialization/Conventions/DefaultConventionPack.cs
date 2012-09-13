using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Bson.Serialization.Conventions
{
    /// <summary>
    /// Convention pack of defaults.
    /// </summary>
    public class DefaultConventionPack : IConventionPack
    {
        private static readonly IConventionPack __defaultConventionPack = ConventionPack.FromConventionProfile(ConventionProfile.GetDefault());

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static IConventionPack Instance
        {
            get { return __defaultConventionPack; }
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="DefaultConventionPack" /> class from being created.
        /// </summary>
        private DefaultConventionPack()
        { }

        /// <summary>
        /// Gets the conventions.
        /// </summary>
        public IEnumerable<IConvention> Conventions
        {
            get { return __defaultConventionPack.Conventions; }
        }
    }
}