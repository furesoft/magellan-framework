using Magellan.Exceptions;
using Magellan.Routing;
using NUnit.Framework;
using System.Text.RegularExpressions;

namespace Magellan.Tests.Routing
{
    [TestFixture]
    public class ParameterSegmentTests
    {
        [Test]
        public void EvaluateValue()
        {
            var segment = new ParameterSegment("controller", "Home", null);
            Assert.IsTrue(segment.MatchPath(null, new PathIterator("/foo")).Success);
            Assert.AreEqual("foo", segment.MatchPath(null, new PathIterator("/foo")).Values["controller"]);
        }

        [Test]
        public void EvaluateDefaulted()
        {
            var segment = new ParameterSegment("controller", "Home", null);
            Assert.IsTrue(segment.MatchPath(null, new PathIterator("")).Success);
            Assert.AreEqual("Home", segment.MatchPath(null, new PathIterator("")).Values["controller"]);
        }

        [Test]
        public void EvaluateConstrained()
        {
            var segment = new ParameterSegment("controller", "Home", "^[0-9]+$");
            Assert.IsFalse(segment.MatchPath(null, new PathIterator("/foo")).Success);
            Assert.IsTrue(segment.MatchPath(null, new PathIterator("/1")).Success);
            Assert.AreEqual("1", segment.MatchPath(null, new PathIterator("/1")).Values["controller"]);
        }

        [Test]
        public void StringConstraintsTreatedAsRegexConstraints()
        {
            var segment = new ParameterSegment("controller", "Home", "^[0-9]+$");
            Assert.IsFalse(segment.MatchPath(null, new PathIterator("/foo")).Success);
            Assert.IsTrue(segment.MatchPath(null, new PathIterator("/1")).Success);
            Assert.AreEqual("1", segment.MatchPath(null, new PathIterator("/1")).Values["controller"]);
        }

        [Test]
        public void RegexConstraintsTreatedAsRegexConstraints()
        {
            var segment = new ParameterSegment("controller", "Home", new Regex("^[0-9]+$"));
            Assert.IsFalse(segment.MatchPath(null, new PathIterator("/foo")).Success);
            Assert.IsTrue(segment.MatchPath(null, new PathIterator("/1")).Success);
            Assert.AreEqual("1", segment.MatchPath(null, new PathIterator("/1")).Values["controller"]);
        }

        [Test]
        public void ConstraintsTreatedAsConstraints()
        {
            var segment = new ParameterSegment("controller", "Home", new RegexConstraint("^[0-9]+$"));
            Assert.IsFalse(segment.MatchPath(null, new PathIterator("/foo")).Success);
            Assert.IsTrue(segment.MatchPath(null, new PathIterator("/1")).Success);
            Assert.AreEqual("1", segment.MatchPath(null, new PathIterator("/1")).Values["controller"]);
        }

        [Test]
        public void NonStringsOrRegexesMustBeContraints()
        {
            Assert.Throws<UnsupportedConstraintException>(
                () => new ParameterSegment("controller", "Home", 35)
                );
        }
    }
}
