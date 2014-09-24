using System;
using System.Windows;
using Magellan.Framework;
using Magellan.Tests.Helpers;
using Magellan.Views;
using Moq;
using NUnit.Framework;

namespace Magellan.Tests.Framework
{
    [TestFixture]
    public class WindowViewEngineResultTests
    {
        #region SUT

        public class DummyView : Window, IModelBound
        {
            public object Model { get; set; }
        }

        protected WindowViewEngineResult CreateResult(Type pageType, object parameters, object model)
        {
            var request = RequestBuilder.CreateRequest();

            var options = new ViewResultOptions(parameters);
            options.Add("Model", model);

            return new WindowViewEngineResult(
                pageType,
                options,
                request.BuildControllerContext(),
                new DefaultViewActivator()
                );
        }

        protected WindowViewEngineResult CreateResult(object page, object parameters, object model)
        {
            var request = RequestBuilder.CreateRequest();
            
            var options = new ViewResultOptions(parameters);
            options.Add("Model", model);

            var viewActivator = new Mock<IViewActivator>();
            viewActivator.Setup(x => x.Instantiate(It.IsAny<Type>())).Returns(page);

            return new WindowViewEngineResult(
                page.GetType(),
                options,
                request.BuildControllerContext(),
                viewActivator.Object
                );
        }

        #endregion

        [Test] 
        public void ShouldShowRenderedWindow()
        {
            var result = CreateResult(typeof (Window), null, null);
            result.Render();
            Assert.IsNotNull(result.RenderedInstance);
            Assert.IsTrue(result.RenderedInstance.IsVisible);
            result.RenderedInstance.Close();
        }

        [Test]
        public void ShouldSetModelAsDataContext()
        {
            var result = CreateResult(typeof(Window), null, "Hello");
            result.Render();
            Assert.AreEqual("Hello", result.RenderedInstance.DataContext);
            result.RenderedInstance.Close();
        }

        [Test]
        public void ShouldSetModelAsModelIfViewImplementsIView()
        {
            var result = CreateResult(typeof(DummyView), null, "Hello");
            result.Render();
            Assert.AreEqual(null, result.RenderedInstance.DataContext);
            Assert.AreEqual("Hello", ((IModelBound)result.RenderedInstance).Model);
            result.RenderedInstance.Close();
        }

        [Test]
        public void ShouldUseViewActivatorToCreateWindow()
        {
            var window = new Window();
            var result = CreateResult(window, null, "Hello");
            result.Render();
            Assert.AreEqual(window, result.RenderedInstance);
            result.RenderedInstance.Close();
        }

        [Test]
        public void ShouldShowAsDialogWhenAsked()
        {
            var shown = false;
            var window = new Window();
            window.Loaded += (x, y) => { shown = true; window.DialogResult = true; };
            var result = CreateResult(window, new { ViewType = "Dialog" }, null); 
            result.Render();
            Assert.IsTrue(shown);
            Assert.IsTrue(window.DialogResult ?? false);
            result.RenderedInstance.Close();
        }
    }
}
