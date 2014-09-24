using System.Text.RegularExpressions;
using Magellan.Exceptions;
using Magellan.Routing;
using NUnit.Framework;

namespace Magellan.Tests.Routing
{
    [TestFixture]
    public class CatchAllParameterSegmentTests
    {
        [Test]
        public void EvaluateValue()
        {
            var segment = new CatchAllParameterSegment("controller", "Home", null);
            Assert.IsTrue(segment.MatchPath(null, new PathIterator("/foo")).Success);
            Assert.IsTrue(segment.MatchPath(null, new PathIterator("/foo/bar/baz")).Success);
            Assert.AreEqual("foo", segment.MatchPath(null, new PathIterator("/foo")).Values["controller"]);
            Assert.AreEqual("foo/bar/baz", segment.MatchPath(null, new PathIterator("/foo/bar/baz")).Values["controller"]);
        }

        [Test]
        public void EvaluateDefaulted()
        {
            var segment = new CatchAllParameterSegment("controller", "Home", null);
            Assert.IsTrue(segment.MatchPath(null, new PathIterator("")).Success);
            Assert.AreEqual("Home", segment.MatchPath(null, new PathIterator("")).Values["controller"]);
        }

        [Test]
        public void EvaluateConstrained()
        {
            var segment = new CatchAllParameterSegment("controller", "Home", "^[0-9]+$");
            Assert.IsFalse(segment.MatchPath(null, new PathIterator("/foo")).Success);
            Assert.IsTrue(segment.MatchPath(null, new PathIterator("/1")).Success);
            Assert.AreEqual("1", segment.MatchPath(null, new PathIterator("/1")).Values["controller"]);
        }

        [Test]
        public void StringConstraintsTreatedAsRegexConstraints()
        {
            var segment = new CatchAllParameterSegment("controller", "Home", "^[0-9]+$");
            Assert.IsFalse(segment.MatchPath(null, new PathIterator("/foo")).Success);
            Assert.IsTrue(segment.MatchPath(null, new PathIterator("/1")).Success);
            Assert.AreEqual("1", segment.MatchPath(null, new PathIterator("/1")).Values["controller"]);
        }

        [Test]
        public void RegexConstraintsTreatedAsRegexConstraints()
        {
            var segment = new CatchAllParameterSegment("controller", "Home", new Regex("^[0-9]+$"));
            Assert.IsFalse(segment.MatchPath(null, new PathIterator("/foo")).Success);
            Assert.IsTrue(segment.MatchPath(null, new PathIterator("/1")).Success);
            Assert.AreEqual("1", segment.MatchPath(null, new PathIterator("/1")).Values["controller"]);
        }

        [Test]
        public void ConstraintsTreatedAsConstraints()
        {
            var segment = new CatchAllParameterSegment("controller", "Home", new RegexConstraint("^[0-9]+$"));
            Assert.IsFalse(segment.MatchPath(null, new PathIterator("/foo")).Success);
            Assert.IsTrue(segment.MatchPath(null, new PathIterator("/1")).Success);
            Assert.AreEqual("1", segment.MatchPath(null, new PathIterator("/1")).Values["controller"]);
        }

        [Test]
        public void NonStringsOrRegexesMustBeContraints()
        {
            Assert.Throws<UnsupportedConstraintException>(
                () => new CatchAllParameterSegment("controller", "Home", 35)
                );
        }
    }
}