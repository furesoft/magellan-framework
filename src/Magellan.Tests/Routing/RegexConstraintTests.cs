using Magellan.Routing;
using NUnit.Framework;

namespace Magellan.Tests.Routing
{
    [TestFixture]
    public class RegexConstraintTests
    {
        [Test]
        public void MatchTest()
        {
            var constraint = new RegexConstraint("^[A-Z]+$");
            Assert.IsTrue(constraint.IsValid(null, "HELLO", null));
            Assert.IsFalse(constraint.IsValid(null, "99", null));
        }

        [Test]
        public void CaseInsensitiveByDefault()
        {
            var constraint = new RegexConstraint("^[A-Z]+$");
            Assert.IsTrue(constraint.IsValid(null, "HelLo", null));
        }
    }
}
