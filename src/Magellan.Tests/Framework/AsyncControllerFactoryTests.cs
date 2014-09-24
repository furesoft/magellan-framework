using Magellan.Framework;
using Magellan.Tests.Helpers;
using Moq;
using NUnit.Framework;

namespace Magellan.Tests.Framework
{
    [TestFixture]
    public class AsyncControllerFactoryTests
    {
        protected ControllerFactory Factory;

        [SetUp]
        public void Setup()
        {
            Factory = new AsyncControllerFactory();
        }

        [Test]
        public void ShouldSetActionInvokerIfControllerBase()
        {
            var controller = new Mock<ControllerBase>();
            
            Factory.Register("Controller1", () => controller.Object);
            var result = Factory.CreateController(RequestBuilder.CreateRequest().BuildRequest(), "Controller1");
            Assert.IsInstanceOf<AsyncActionInvoker>(((ControllerBase)result.Controller).ActionInvoker);
        }

        [Test]
        public void ShouldIgnoreNonControllerBase()
        {
            var controller = new Mock<IController>();

            Factory.Register("Controller1", () => controller.Object);
            var result = Factory.CreateController(RequestBuilder.CreateRequest().BuildRequest(), "Controller1");
            Assert.IsNotNull(result.Controller);
        }
    }
}
