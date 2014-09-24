using Magellan.Exceptions;
using Magellan.Routing;
using Magellan.Tests.Helpers;
using NUnit.Framework;

namespace Magellan.Tests.Routing
{
    [TestFixture]
    public class RouteValidatorTests
    {
        [SetUp]
        public void SetUp()
        {
            Routes = new TestRouteCatalog();
        }

        protected TestRouteCatalog Routes { get; private set; }

        [Test]
        public void ValidationShouldPickUpCommonRouteErrorsAtRegistration()
        {
            
        }

        [Test]
        public void EnsuresParameterNamesAreUnique()
        {
            Assert.Throws<InvalidRouteException>(() => Routes.Register("blog/{id}/{id}"));
            Assert.Throws<InvalidRouteException>(() => Routes.Register("blog/{ID}/{*id}"));
            Assert.Throws<InvalidRouteException>(() => Routes.Register("blog/{*Foo}/{*foo}"));
        }

        [Test]
        public void EnsureCatchAllOnlyAppearAtEnd()
        {
            Assert.Throws<InvalidRouteException>(() => Routes.Register("blog/{*path}/hello"));
        }

        [Test]
        public void EnsureOnlyOneCatchAll()
        {
            Assert.Throws<InvalidRouteException>(() => Routes.Register("blog/{*foo}/{*bar}"));
        }

        [Test]
        public void EnsureRecognizableSegments()
        {
            Assert.Throws<InvalidRouteException>(() => Routes.Register("{sddsds/foosdd}"));
        }

        [Test]
        public void ExceptionGivesDetailedErrors()
        {
            var ex = Assert.Throws<InvalidRouteException>(() => Routes.Register("blog/{id}/{id}"));

            Assert.IsTrue(ex.Message.Contains("The following parameters appeared more than once: 'id'"));
            Assert.IsInstanceOf<Route>(ex.Route);
            Assert.IsFalse(ex.Result.Success);
            Assert.IsTrue(ex.Result.Errors[0].Contains("The following parameters appeared more than once: 'id'"));
        }
    }
}
