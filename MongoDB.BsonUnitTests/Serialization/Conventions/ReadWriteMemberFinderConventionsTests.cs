using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.BsonUnitTests.Serialization.Conventions
{
    [TestFixture]
    public class ReadWriteMemberFinderConventionsTests
    {
        private ReadWriteMemberFinderConvention _subject;

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            _subject = new ReadWriteMemberFinderConvention();
        }

        [Test]
        public void TestMapsAllTheReadAndWriteFieldsAndProperties()
        {
            var classMap = new BsonClassMap<TestClass>();

            _subject.Apply(classMap);

            Assert.AreEqual(3, classMap.DeclaredMemberMaps.Count());

            Assert.IsNotNull(classMap.GetMemberMap(x => x.Mapped1));
            Assert.IsNotNull(classMap.GetMemberMap(x => x.Mapped2));
            Assert.IsNotNull(classMap.GetMemberMap(x => x.Mapped3));

            Assert.IsNull(classMap.GetMemberMap(x => x.NotMapped1));
            Assert.IsNull(classMap.GetMemberMap(x => x.NotMapped2));
        }

        private class TestClass
        {
            public string Mapped1 { get; set; }

            public string Mapped2 = "blah";

            // yes, we'll map this because we know how to set it and part of it is public...
            public string Mapped3 { get; private set; }

            public readonly string NotMapped1 = "blah";

            public string NotMapped2
            {
                get { return "blah"; }
            }
        }
    }
}