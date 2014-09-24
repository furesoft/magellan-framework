using System;
using Magellan.Routing;
using NUnit.Framework;

namespace Magellan.Tests.Routing
{
    [TestFixture]
    public class LiteralSegmentTests
    {
        [Test]
        public void PositiveEvaluateTest()
        {
            var segment = new LiteralSegment("baz");
            Assert.IsTrue(segment.MatchPath(null, new PathIterator("/baz")).Success);
        }

        [Test]
        public void PathIsNotCaseSensitive()
        {
            var segment = new LiteralSegment("baz");
            Assert.IsTrue(segment.MatchPath(null, new PathIterator("/BaZ")).Success);
        }

        [Test]
        public void NegativeEvaluateTest()
        {
            var segment = new LiteralSegment("zamoo");
            Assert.IsFalse(segment.MatchPath(null, new PathIterator("/baz")).Success);
        }

        [Test]
        public void ParameterNameIsRequired()
        {
            Assert.Throws<ArgumentException>(() => new LiteralSegment("  "));
            Assert.Throws<ArgumentException>(() => new LiteralSegment(""));
            Assert.Throws<ArgumentNullException>(() => new LiteralSegment(null));
        }
    }
}
