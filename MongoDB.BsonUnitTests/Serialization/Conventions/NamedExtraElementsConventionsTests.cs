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
    public class NamedExtraElementsConventionsTests
    {
        private NamedExtraElementsConvention _subject;

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            _subject = new NamedExtraElementsConvention(new[] { "One", "Two" });
        }

        [Test]
        public void TestDoesNotMapExtraElementsWhenOneIsNotFound()
        {
            var classMap = new BsonClassMap<TestClass1>();

            _subject.Apply(classMap);

            Assert.IsNull(classMap.ExtraElementsMemberMap);
        }

        [Test]
        public void TestMapsExtraElementsWhenFirstNameExists()
        {
            var classMap = new BsonClassMap<TestClass2>();

            _subject.Apply(classMap);

            Assert.IsNotNull(classMap.ExtraElementsMemberMap);
        }

        [Test]
        public void TestMapsExtraElementsWhenSecondNameExists()
        {
            var classMap = new BsonClassMap<TestClass3>();

            _subject.Apply(classMap);

            Assert.IsNotNull(classMap.ExtraElementsMemberMap);
        }

        [Test]
        public void TestMapsExtraElementsWhenBothExist()
        {
            var classMap = new BsonClassMap<TestClass4>();

            _subject.Apply(classMap);

            Assert.IsNotNull(classMap.ExtraElementsMemberMap);
            Assert.AreEqual("One", classMap.ExtraElementsMemberMap.MemberName);
        }

        [Test]
        public void TestDoesNotMapExtraElementsWhenIsNotValidType()
        {
            var classMap = new BsonClassMap<TestClass5>();

            _subject.Apply(classMap);

            Assert.IsNull(classMap.ExtraElementsMemberMap);
        }

        private class TestClass1
        {
            public BsonDocument None { get; set; }
        }

        private class TestClass2
        {
            public BsonDocument One { get; set; }
        }

        private class TestClass3
        {
            public BsonDocument Two { get; set; }
        }

        private class TestClass4
        {
            public BsonDocument One { get; set; }

            public BsonDocument Two { get; set; }
        }

        private class TestClass5
        {
            public int One { get; set; }
        }
    }
}