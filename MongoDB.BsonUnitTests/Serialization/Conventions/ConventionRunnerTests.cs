using MongoDB.Bson.Serialization.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MongoDB.Bson.Serialization;
using System.Threading;
using System.Diagnostics;

namespace MongoDB.BsonUnitTests.Serialization.Conventions
{
    [TestFixture]
    public class ConventionRunnerTests
    {
        private ConventionPack _pack;
        private ConventionRunner _subject;

        [SetUp]
        public void SetUp()
        {
            var stopwatch = new Stopwatch();
            _pack = new ConventionPack();
            _pack.AddRange(new IConvention[] 
            {
                new TrackingBeforeConvention(stopwatch) { Name = "1" },
                new TrackingMemberConvention(stopwatch) { Name = "3" },
                new TrackingAfterConvention(stopwatch) { Name = "5" },
                new TrackingMemberConvention(stopwatch) { Name = "4" },
                new TrackingAfterConvention(stopwatch) { Name = "6" },
                new TrackingBeforeConvention(stopwatch) { Name = "2" },
            });
            _subject = new ConventionRunner(_pack);

            var classMap = new BsonClassMap<TestClass>(cm =>
            {
                cm.MapMember(t => t.Prop1);
                cm.MapMember(t => t.Prop2);
            });

            stopwatch.Start();
            _subject.Apply(classMap);
            stopwatch.Stop();
        }

        [Test]
        public void TestThatItRunsAllConventions()
        {
            var allRun = _pack.Conventions.OfType<ITrackRun>().All(x => x.IsRun);
            Assert.IsTrue(allRun);
        }

        [Test]
        public void TestThatItRunsConventionsInTheProperOrder()
        {
            var conventions = _pack.Conventions.OfType<ITrackRun>().OrderBy(x => x.RunTicks).ToList();
            for (int i = 1; i < conventions.Count; i++)
            {
                if (conventions[i - 1].Name != i.ToString())
                {
                    Assert.Fail("Convention ran out of order. Expected {0} but was {1}.", conventions[0].Name, i);
                }
            }
        }

        [Test]
        public void TestThatItRunsBeforeMemberConventionsOnceEach()
        {
            var beforeConventions = _pack.Conventions.OfType<IBeforeMembersBsonClassMapConvention>().OfType<ITrackRun>();
            var allAreRunOnce = beforeConventions.All(x => x.RunCount == 1);

            Assert.IsTrue(allAreRunOnce);
        }

        [Test]
        public void TestThatItRunsAfterMemberConventionsOnceEach()
        {
            var afterConventions = _pack.Conventions.OfType<IAfterMembersBsonClassMapConvention>().OfType<ITrackRun>();
            var allAreRunOnce = afterConventions.All(x => x.RunCount == 1);

            Assert.IsTrue(allAreRunOnce);
        }

        [Test]
        public void TestThatItRunsMemberConventionsTwiceEach()
        {
            var memberConventions = _pack.Conventions.OfType<IBsonMemberMapConvention>().OfType<ITrackRun>();
            var allAreRunTwice = memberConventions.All(x => x.RunCount == 2);

            Assert.IsTrue(allAreRunTwice);
        }

        private class TestClass
        {
            public string Prop1 { get; set; }

            public string Prop2 { get; set; }
        }

        private interface ITrackRun
        {
            bool IsRun { get; }

            string Name { get; }

            int RunCount { get; }

            long RunTicks { get; }
        }

        private class TrackingBeforeConvention : IBeforeMembersBsonClassMapConvention, ITrackRun
        {
            private readonly Stopwatch _stopwatch;

            public TrackingBeforeConvention(Stopwatch stopwatch)
            {
                _stopwatch = stopwatch;
            }

            public bool IsRun { get; set; }

            public int RunCount { get; set; }

            public long RunTicks { get; set; }

            public string Name { get; set; }

            public void Apply(BsonClassMap classMap)
            {
                IsRun = true;
                RunCount++;
                RunTicks = _stopwatch.ElapsedTicks;
            }
        }

        private class TrackingMemberConvention : IBsonMemberMapConvention, ITrackRun
        {
            private readonly Stopwatch _stopwatch;

            public TrackingMemberConvention(Stopwatch stopwatch)
            {
                _stopwatch = stopwatch;
            }

            public bool IsRun { get; set; }

            public int RunCount { get; set; }

            public long RunTicks { get; set; }

            public string Name { get; set; }

            public void Apply(BsonMemberMap memberMap)
            {
                IsRun = true;
                RunCount++;
                RunTicks = _stopwatch.ElapsedTicks;
            }
        }

        private class TrackingAfterConvention : IAfterMembersBsonClassMapConvention, ITrackRun
        {
            private readonly Stopwatch _stopwatch;

            public TrackingAfterConvention(Stopwatch stopwatch)
            {
                _stopwatch = stopwatch;
            }

            public bool IsRun { get; set; }

            public int RunCount { get; set; }

            public long RunTicks { get; set; }

            public string Name { get; set; }

            public void Apply(BsonClassMap classMap)
            {
                IsRun = true;
                RunCount++;
                RunTicks = _stopwatch.ElapsedTicks;
            }
        }
    }
}