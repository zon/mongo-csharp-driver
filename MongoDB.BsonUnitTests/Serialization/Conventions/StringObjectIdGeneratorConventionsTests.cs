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
    public class StringObjectIdIdGeneratorConventionsTests
    {
        private StringObjectIdIdGeneratorConvention _subject;

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            _subject = new StringObjectIdIdGeneratorConvention();
        }

        [Test]
        public void TestDoesNotSetWhenNoIdExists()
        {
            var classMap = new BsonClassMap<TestClass>(cm =>
            { });

            Assert.DoesNotThrow(() => _subject.Apply(classMap));
        }

        [Test]
        public void TestDoesNotSetWhenTypeIsntString()
        {
            var classMap = new BsonClassMap<TestClass>(cm =>
            {
                cm.MapIdMember(x => x.OId);
            });

            _subject.Apply(classMap);
            Assert.IsNull(classMap.IdMemberMap.IdGenerator);
        }

        [Test]
        public void TestDoesNotSetWhenStringIsNotRepresentedAsObjectId()
        {
            var classMap = new BsonClassMap<TestClass>(cm =>
            {
                cm.MapIdMember(x => x.String);
            });

            _subject.Apply(classMap);
            Assert.IsNull(classMap.IdMemberMap.IdGenerator);
        }

        [Test]
        public void TestSetsWhenIdIsStringAndRepresentedAsAnObjectId()
        {
            var classMap = new BsonClassMap<TestClass>(cm =>
            {
                cm.MapIdMember(x => x.String).SetRepresentation(BsonType.ObjectId);
            });

            _subject.Apply(classMap);
            Assert.IsNotNull(classMap.IdMemberMap.IdGenerator);
            Assert.IsInstanceOf<StringObjectIdGenerator>(classMap.IdMemberMap.IdGenerator);
        }

        private class TestClass
        {
            public ObjectId OId { get; set; }

            public string String { get; set; }
        }
    }
}