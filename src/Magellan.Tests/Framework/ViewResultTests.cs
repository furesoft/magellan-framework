using System.Linq;
using Magellan.Exceptions;
using Magellan.Framework;
using Magellan.Tests.Helpers;
using Moq;
using NUnit.Framework;

namespace Magellan.Tests.Framework
{
    [TestFixture]
    public class ViewResultTests
    {
        #region SUT

        public class DummyViewResult : ViewResult
        {
            public DummyViewResult(string viewName, ViewEngineCollection viewEngines) : base(viewName, null, viewEngines)
            {
            }
            public DummyViewResult(string viewName, object model, ViewEngineCollection viewEngines)
                : base(viewName, model, viewEngines)
            {
            }
        }

        #endregion

        [Test]
        public void ShouldAllowNullViewName()
        {
            new DummyViewResult(null, new ViewEngineCollection());
        }

        [Test]
        public void ShouldSetModel()
        {
            var result = new DummyViewResult(null, "Hello world", new ViewEngineCollection());
            Assert.AreEqual("Hello world", result.Options["Model"]);       
        }

        [Test]
        public void ShouldInferNullViewNameFromAction()
        {
            var viewEngine = new Mock<IViewEngine>();
            viewEngine.Setup(x => x.FindView(It.IsAny<ControllerContext>(), It.IsAny<ViewResultOptions>(), It.IsAny<string>())).Returns(new ViewEngineResult(true, null));

            var result = new DummyViewResult(null, new ViewEngineCollection(viewEngine.Object));
            var request = RequestBuilder.CreateRequest("MyController", "MyAction", null).BuildControllerContext();

            result.Execute(request);
            viewEngine.Verify(x => x.FindView(request, It.IsAny<ViewResultOptions>(), "MyAction"));
        }

        [Test]
        public void ShouldPassViewNameToFindView()
        {
            var viewEngine = new Mock<IViewEngine>();
            viewEngine.Setup(x => x.FindView(It.IsAny<ControllerContext>(), It.IsAny<ViewResultOptions>(), It.IsAny<string>())).Returns(new ViewEngineResult(true, null));

            var result = new DummyViewResult("MyView", new ViewEngineCollection(viewEngine.Object));
            var request = RequestBuilder.CreateRequest("MyController", "MyAction", null).BuildControllerContext();

            result.Execute(request);
            viewEngine.Verify(x => x.FindView(request, It.IsAny<ViewResultOptions>(), "MyView"));
        }

        [Test]
        public void ShouldThrowWhenViewNotFound()
        {
            var viewEngine = new Mock<IViewEngine>();
            viewEngine.Setup(x => x.FindView(It.IsAny<ControllerContext>(), It.IsAny<ViewResultOptions>(), It.IsAny<string>())).Returns(new ViewEngineResult(false, new string[] { "ABC" }));

            var result = new DummyViewResult("MyView", new ViewEngineCollection(viewEngine.Object));
            var request = RequestBuilder.CreateRequest("MyController", "MyAction", null).BuildControllerContext();

            var ex = Assert.Throws<ViewNotFoundException>(() => result.Execute(request));
            Assert.IsTrue(ex.SearchLocations.Contains("ABC"));
            Assert.AreEqual("MyView", ex.ViewName);            
        }
    }
}
