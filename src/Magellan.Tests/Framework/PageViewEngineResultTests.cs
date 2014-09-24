using System;
using System.Windows.Controls;
using Magellan.Framework;
using Magellan.Routing;
using Magellan.Testability;
using Magellan.Tests.Helpers;
using Magellan.Views;
using Moq;
using NUnit.Framework;

namespace Magellan.Tests.Framework
{
    [TestFixture]
    public class PageViewEngineResultTests
    {
        #region SUT

        public class DummyView : Page, IModelBound
        {
            public object Model { get; set; }
        }

        #endregion

        [SetUp]
        public void SetUp()
        {
            Navigator = new Mock<INavigator>();
        }

        protected PageViewEngineResult CreateResult(Type pageType, object parameters, object model)
        {
            var request = RequestBuilder.CreateRequest("X", "Y", parameters);
            request.Navigator = Navigator;
            request.Navigator.SetupGet(x => x.Dispatcher).Returns(new SingleThreadDispatcher());
            
            var options = new ViewResultOptions(parameters);
            options.SetModel(model);

            return new PageViewEngineResult(
                pageType,
                options, 
                request.BuildControllerContext(),
                new DefaultViewActivator()
                );
        }

        protected PageViewEngineResult CreateResult(object page, object parameters, object model)
        {
            var request = RequestBuilder.CreateRequest("X", "Y", parameters);
            request.Navigator = Navigator;
            request.Navigator.SetupGet(x => x.Dispatcher).Returns(new SingleThreadDispatcher());

            var options = new ViewResultOptions(parameters);
            options.SetModel(model);

            var viewActivator = new Mock<IViewActivator>();
            viewActivator.Setup(x => x.Instantiate(It.IsAny<Type>())).Returns(page);

            return new PageViewEngineResult(
                page.GetType(),
                options,
                request.BuildControllerContext(),
                viewActivator.Object
                );
        }

        protected Mock<INavigator> Navigator { get; set; }

        [Test]
        public void ShouldShowRenderedPage()
        {
            var result = CreateResult(typeof (Page), null, null);
            result.Render();
            Assert.IsNotNull(result.RenderedInstance);
            Navigator.Verify(x => x.NavigateDirectToContent(It.IsAny<Page>(), It.IsAny<ResolvedNavigationRequest>()));
        }

        [Test]
        public void ShouldSetModelAsDataContext()
        {
            var result = CreateResult(typeof (Page), null, "Hello");
            result.Render();
            Assert.AreEqual("Hello", result.RenderedInstance.DataContext);
            Navigator.Verify(x => x.NavigateDirectToContent(It.IsAny<Page>(), It.IsAny<ResolvedNavigationRequest>()));
        }

        [Test]
        public void ShouldSetModelAsModelIfViewImplementsIView()
        {
            var result = CreateResult(typeof(DummyView), null, "Hello");
            result.Render();
            Assert.AreEqual(null, result.RenderedInstance.DataContext);
            Assert.AreEqual("Hello", ((IModelBound)result.RenderedInstance).Model);
            Navigator.Verify(x => x.NavigateDirectToContent(It.IsAny<DummyView>(), It.IsAny<ResolvedNavigationRequest>()));
        }

        [Test]
        public void ShouldUseViewActivatorToCreatePage()
        {
            var page = new Page();
            var result = CreateResult(page, null, "Hello");
            result.Render();
            Assert.AreEqual(page, result.RenderedInstance);
            Assert.AreEqual("Hello", result.RenderedInstance.DataContext);
            Navigator.Verify(x => x.NavigateDirectToContent(It.IsAny<Page>(), It.IsAny<ResolvedNavigationRequest>()));
        }

        [Test]
        public void ShouldResetNavigationHistoryWhenAsked()
        {
            var result = CreateResult(typeof(Page), new { ResetNavigationHistory = true }, "Hello");
            result.Render();
            Assert.AreEqual("Hello", result.RenderedInstance.DataContext);
            Navigator.Verify(x => x.NavigateDirectToContent(It.IsAny<Page>(), It.IsAny<ResolvedNavigationRequest>()));
            Navigator.Verify(x => x.ResetHistory());
        }
    }
}