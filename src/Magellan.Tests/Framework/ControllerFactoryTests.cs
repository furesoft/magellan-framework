using System;
using Magellan.Framework;
using Magellan.Tests.Helpers;
using Moq;
using NUnit.Framework;

namespace Magellan.Tests.Framework
{
    [TestFixture]
    public class ControllerFactoryTests
    {
        protected ControllerFactory Factory;

        [SetUp]
        public void Setup()
        {
            Factory = new ControllerFactory();
        }

        [Test]
        public void ControllerNameAndBuilderAreRequired()
        {
            Assert.Throws<ArgumentNullException>(() => Factory.Register(null, () => null));
            Assert.Throws<ArgumentNullException>(() => Factory.Register("Foo", null));
        }

        [Test]
        public void ControllerNamesMustBeUnique()
        {
            Factory.Register("Bar", () => null);
            Assert.Throws<ArgumentException>(() => Factory.Register("Bar", () => null));
        }

        [Test]
        public void ControllerNamesAreNotCaseSensitiveWhenRegistering()
        {
            Factory.Register("Foo", () => null);
            Assert.Throws<ArgumentException>(() => Factory.Register("FOO", () => null));
        }

        [Test]
        public void ControllerBuilderCallbackShouldNotBeInvokedUntilResolution()
        {
            Factory.Register("Foo", () =>
            {
                Assert.Fail("This should not have been called");
                return null;
            });
        }

        [Test]
        public void ShouldCreateAppropriateControllerByName()
        {
            var controller1 = new Mock<IController>();
            var controller2 = new Mock<IController>();

            Factory.Register("Controller1", () => controller1.Object);
            Factory.Register("Controller2", () => controller2.Object);

            Assert.AreSame(controller1.Object, Factory.CreateController(RequestBuilder.CreateRequest().BuildRequest(), "Controller1").Controller);
            Assert.AreSame(controller2.Object, Factory.CreateController(RequestBuilder.CreateRequest().BuildRequest(), "Controller2").Controller);
            Assert.AreNotSame(controller1.Object, controller2.Object);
        }

        [Test]
        public void ControllerNamesAreNotCaseSensitiveWhenResolving()
        {
            var controller1 = new Mock<IController>();
            Factory.Register("Controller1", () => controller1.Object);
            Factory.CreateController(RequestBuilder.CreateRequest().BuildRequest(), "Controller1");
            Factory.CreateController(RequestBuilder.CreateRequest().BuildRequest(), "conTROLLer1");
        }

        [Test]
        public void ShouldThrowOnUnrecognisedController()
        {
            Assert.Throws<ArgumentException>(() => Factory.CreateController(RequestBuilder.CreateRequest().BuildRequest(), "OompaLoompa"));
        }

        [Test]
        public void ShouldDisposeDisposableControllers()
        {
            var controller = new Mock<IController>();
            var disposableController = controller.As<IDisposable>();
            disposableController.Setup(x => x.Dispose()).AtMostOnce();

            Factory.Register("Controller1", () => controller.Object);
            var result = Factory.CreateController(RequestBuilder.CreateRequest().BuildRequest(), "Controller1");
            result.Dispose();

            disposableController.Verify(x => x.Dispose());
        }
    }
}
