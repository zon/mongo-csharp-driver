using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MongoDB.Bson.Serialization.Conventions
{
    /// <summary>
    /// Convention pack of defaults.
    /// </summary>
    public class DefaultConventionPack : IConventionPack
    {
        private static readonly IConventionPack __defaultConventionPack = new DefaultConventionPack();
        private readonly IEnumerable<IConvention> _conventions;

        /// <summary>
        /// Prevents a default instance of the <see cref="DefaultConventionPack" /> class from being created.
        /// </summary>
        private DefaultConventionPack()
        {
            _conventions = new List<IConvention>
            {
                new ReadWriteMemberFinderConvention(),
                new NamedIdMemberConvention(new [] { "Id", "id", "_id" }),
                new NamedExtraElementsMemberConvention(new [] { "ExtraElements" }),
                new NeverIgnoreExtraElementsConvention(),
                new LookupIdGeneratorConvention(),
                new StringObjectIdIdGeneratorConvention()
            };
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static IConventionPack Instance
        {
            get { return __defaultConventionPack; }
        }

        /// <summary>
        /// Gets the conventions.
        /// </summary>
        public IEnumerable<IConvention> Conventions
        {
            get { return _conventions; }
        }
    }
}