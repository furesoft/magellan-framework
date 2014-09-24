using Magellan.Exceptions;
using Magellan.Framework;
using Moq;
using NUnit.Framework;

namespace Magellan.Tests.Framework
{
    [TestFixture]
    public class ControllerRouteValidatorTests
    {
        [SetUp]
        public void SetUp()
        {
            Catalog = new ControllerRouteCatalog(new Mock<IControllerFactory>().Object);
            Catalog.Validator = new ControllerRouteValidator();
        }

        protected ControllerRouteCatalog Catalog { get; private set; }

        [Test]
        public void InheritsRulesFromBase()
        {
            Assert.Throws<InvalidRouteException>(() => Catalog.MapRoute("@!@&*@!"));
        }

        [Test]
        public void EnsuresControllerMustBeSpecified()
        {
            // Invalid
            var ex = Assert.Throws<InvalidRouteException>(() => Catalog.MapRoute("customers"));
            Assert.IsTrue(ex.Message.Contains("The route does not contain a '{controller}' segment"));

            // Valid
            Catalog.MapRoute("customers/{controller}/{action}");
            Catalog.MapRoute("customers", new { controller = "bar", action = "foo" });
        }

        [Test]
        public void EnsuresActionMustBeSpecified()
        {
            // Invalid
            var ex = Assert.Throws<InvalidRouteException>(() => Catalog.MapRoute("customers/{controller}"));
            Assert.IsTrue(ex.Message.Contains("The route does not contain an '{action}' segment"));

            // Valid
            Catalog.MapRoute("customers/{controller}/{action}");
            Catalog.MapRoute("customers/{controller}", new { action = "foo"});
        }
    }
}