using Magellan.Exceptions;
using Magellan.Framework;
using Moq;
using NUnit.Framework;

namespace Magellan.Tests.Framework
{
    [TestFixture]
    public class ViewModelRouteValidatorTests
    {
        [SetUp]
        public void SetUp()
        {
            Catalog = new ViewModelRouteCatalog(new Mock<IViewModelFactory>().Object);
            Catalog.Validator = new ViewModelRouteValidator();
        }

        protected ViewModelRouteCatalog Catalog { get; private set; }

        [Test]
        public void InheritsRulesFromBase()
        {
            Assert.Throws<InvalidRouteException>(() => Catalog.MapRoute("@!@&*@!"));
        }

        [Test]
        public void EnsuresViewModelMustBeSpecified()
        {
            // Invalid
            var ex = Assert.Throws<InvalidRouteException>(() => Catalog.MapRoute("home"));
            Assert.IsTrue(ex.Message.Contains("The route does not contain a '{viewModel}' segment"));

            // Valid
            Catalog.MapRoute("home/{viewModel}");
            Catalog.MapRoute("home", new { viewModel = "Customers" });
        }
    }
}