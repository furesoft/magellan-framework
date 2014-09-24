using System.Linq;
using Magellan.Exceptions;
using Magellan.Framework;
using Magellan.Tests.Helpers;
using Moq;
using NUnit.Framework;

namespace Magellan.Tests.Framework
{
    [TestFixture]
    public class ViewEngineCollectionTests
    {
        [Test]
        public void MustRegisterViewEnginesBeforeFindingViews()
        {
            var collection = new ViewEngineCollection();
            var context = RequestBuilder.CreateRequest().BuildControllerContext();
            var parameters = new ViewResultOptions();

            Assert.Throws<NavigationConfigurationException>(() => collection.FindView(context, parameters, "Foo"));
        }

        [Test]
        public void AsksEachViewEngineInTheOrderItWasCalled()
        {
            var redEngine = new Mock<IViewEngine>();
            var redResult = new ViewEngineResult(false, new[] { "Whatever"});
            redEngine.Setup(x => x.FindView(It.IsAny<ControllerContext>(), It.IsAny<ViewResultOptions>(), It.IsAny<string>())).Returns(redResult);

            var orangeEngine = new Mock<IViewEngine>();
            orangeEngine.Setup(x => x.FindView(It.IsAny<ControllerContext>(), It.IsAny<ViewResultOptions>(), It.IsAny<string>())).Returns((ViewEngineResult)null);

            var blueEngine = new Mock<IViewEngine>();
            var blueResult = new ViewEngineResult(true, new string[0]);
            blueEngine.Setup(x => x.FindView(It.IsAny<ControllerContext>(), It.IsAny<ViewResultOptions>(), It.IsAny<string>())).Returns(blueResult);

            var greenEngine = new Mock<IViewEngine>();
            var greenResult = new ViewEngineResult(true, new string[0]);
            greenEngine.Setup(x => x.FindView(It.IsAny<ControllerContext>(), It.IsAny<ViewResultOptions>(), It.IsAny<string>())).Returns(greenResult);

            var collection = new ViewEngineCollection();
            collection.Add(redEngine.Object);
            collection.Add(orangeEngine.Object);
            collection.Add(blueEngine.Object);
            collection.Add(greenEngine.Object);
            var result = collection.FindView(RequestBuilder.CreateRequest().BuildControllerContext(), new ViewResultOptions(), "Foo");
            Assert.AreEqual(blueResult, result);
        }

        [Test]
        public void CombinesAllErrorsWhenResultIsNotFound()
        {
            var redEngine = new Mock<IViewEngine>();
            var redResult = new ViewEngineResult(false, new[] { "red whatever" });
            redEngine.Setup(x => x.FindView(It.IsAny<ControllerContext>(), It.IsAny<ViewResultOptions>(), It.IsAny<string>())).Returns(redResult);

            var blueEngine = new Mock<IViewEngine>();
            var blueResult = new ViewEngineResult(false, new[] { "blue whatever" });
            blueEngine.Setup(x => x.FindView(It.IsAny<ControllerContext>(), It.IsAny<ViewResultOptions>(), It.IsAny<string>())).Returns(blueResult);

            var collection = new ViewEngineCollection();
            collection.Add(redEngine.Object);
            collection.Add(blueEngine.Object);
            var result = collection.FindView(RequestBuilder.CreateRequest().BuildControllerContext(), new ViewResultOptions(), "Foo");
            Assert.IsFalse(result.Success);
            Assert.AreEqual(2, result.SearchLocations.Count());
            Assert.AreEqual("red whatever", result.SearchLocations.ElementAt(0));
            Assert.AreEqual("blue whatever", result.SearchLocations.ElementAt(1));
        }
    }
}
