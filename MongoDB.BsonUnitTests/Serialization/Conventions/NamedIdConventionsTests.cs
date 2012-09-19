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
    public class NamedIdConventionsTests
    {
        private NamedIdConvention _subject;

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            _subject = new NamedIdConvention(new[] { "One", "Two" });
        }

        [Test]
        public void TestDoesNotMapIdWhenOneIsNotFound()
        {
            var classMap = new BsonClassMap<TestClass1>();

            _subject.Apply(classMap);

            Assert.IsNull(classMap.IdMemberMap);
        }

        [Test]
        public void TestMapsIdWhenFirstNameExists()
        {
            var classMap = new BsonClassMap<TestClass2>();

            _subject.Apply(classMap);

            Assert.IsNotNull(classMap.IdMemberMap);
        }

        [Test]
        public void TestMapsIdWhenSecondNameExists()
        {
            var classMap = new BsonClassMap<TestClass3>();

            _subject.Apply(classMap);

            Assert.IsNotNull(classMap.IdMemberMap);
        }

        [Test]
        public void TestMapsIdWhenBothExist()
        {
            var classMap = new BsonClassMap<TestClass4>();

            _subject.Apply(classMap);

            Assert.IsNotNull(classMap.IdMemberMap);
            Assert.AreEqual("One", classMap.IdMemberMap.MemberName);
        }

        private class TestClass1
        {
            public int None { get; set; }
        }

        private class TestClass2
        {
            public int One { get; set; }
        }

        private class TestClass3
        {
            public int Two { get; set; }
        }

        private class TestClass4
        {
            public int One { get; set; }

            public int Two { get; set; }
        }
    }
}