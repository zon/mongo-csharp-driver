using MongoDB.Bson.Serialization.Conventions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.BsonUnitTests.Serialization.Conventions
{
    [TestFixture]
    public class ConventionPackTests
    {
        private ConventionPack _pack;

        [SetUp]
        public void SetUp()
        {
            _pack = new ConventionPack();
        }

        [Test]
        public void TestAdd()
        {
            _pack.Add(new TestConvention { Name = "One" });
            _pack.Add(new TestConvention { Name = "Two" });

            Assert.AreEqual(2, _pack.Conventions.Count());
        }

        [Test]
        public void TestAddRange()
        {
            _pack.AddRange(new IConvention[] 
            {
                new TestConvention { Name = "One" },
                new TestConvention { Name = "Two" }
            });

            Assert.AreEqual(2, _pack.Conventions.Count());
        }

        [Test]
        public void TestAppend()
        {
            _pack.AddRange(new IConvention[] 
            {
                new TestConvention { Name = "One" },
                new TestConvention { Name = "Two" }
            });

            var newPack = new ConventionPack();
            newPack.AddRange(new IConvention[] 
            {
                new TestConvention { Name = "Three" },
                new TestConvention { Name = "Four" }
            });

            _pack.Append(newPack);

            Assert.AreEqual(4, _pack.Conventions.Count());
            Assert.AreEqual("One", _pack.Conventions.ElementAt(0).Name);
            Assert.AreEqual("Two", _pack.Conventions.ElementAt(1).Name);
            Assert.AreEqual("Three", _pack.Conventions.ElementAt(2).Name);
            Assert.AreEqual("Four", _pack.Conventions.ElementAt(3).Name);
        }

        [Test]
        public void TestInsertAfter()
        {
            _pack.AddRange(new IConvention[] 
            {
                new TestConvention { Name = "One" },
                new TestConvention { Name = "Two" }
            });

            _pack.InsertAfter("One", new TestConvention { Name = "Three" });

            Assert.AreEqual(3, _pack.Conventions.Count());
            Assert.AreEqual("Three", _pack.Conventions.ElementAt(1).Name);
        }

        [Test]
        public void TestInsertBefore()
        {
            _pack.AddRange(new IConvention[] 
            {
                new TestConvention { Name = "One" },
                new TestConvention { Name = "Two" }
            });

            _pack.InsertBefore("Two", new TestConvention { Name = "Three" });
            Assert.AreEqual(3, _pack.Conventions.Count());
            Assert.AreEqual("Three", _pack.Conventions.ElementAt(1).Name);
        }

        [Test]
        public void TestRemove()
        {
            _pack.AddRange(new IConvention[] 
            {
                new TestConvention { Name = "One" },
                new TestConvention { Name = "Two" }
            });

            _pack.Remove("Two");
            Assert.AreEqual(1, _pack.Conventions.Count());
            Assert.AreEqual("One", _pack.Conventions.Single().Name);
        }

        private class TestConvention : IConvention
        {
            public string Name { get; set; }
        }

    }
}