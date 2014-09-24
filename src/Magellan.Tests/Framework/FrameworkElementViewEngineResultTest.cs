using System.Windows;
using Magellan.Framework;
using Magellan.Tests.Helpers;
using Magellan.Views;
using NUnit.Framework;

namespace Magellan.Tests.Framework
{
    [TestFixture]
    public class FrameworkElementViewEngineResultTest
    {
        #region SUT

        public class SampleViewEngineResult<TView> : FrameworkElementViewEngineResult
            where TView : FrameworkElement, new()
        {
            public SampleViewEngineResult(ViewResultOptions viewParameters, ControllerContext controllerContext)
                : base(controllerContext, viewParameters)
            {
            }

            public override void Render()
            {
                View = new TView();
                ViewInitializer.Prepare(View, Model, ControllerContext.Request);
            }

            public TView View { get; private set; }
        }

        public class DumbView : FrameworkElement
        {
        }

        public class SmartView : FrameworkElement, IModelBound
        {
            public object Model { get; set; }
        }

        public class ReallySmartView : FrameworkElement, IModelBound, INavigationAware
        {
            public object Model { get; set; }
            public INavigator Navigator { get; set; }
        }

        #endregion

        [Test]
        public void ShouldSetModelIfIViewIsImplemented()
        {
            var request = RequestBuilder.CreateRequest();

            var result = new SampleViewEngineResult<SmartView>(new ViewResultOptions(new { Model = "hello" }), request.BuildControllerContext());
            result.Render();

            Assert.AreEqual("hello", result.View.Model);
        }

        [Test]
        public void ShouldSetDataContextIfIViewIsNotImplemented()
        {
            var request = RequestBuilder.CreateRequest();

            var result = new SampleViewEngineResult<DumbView>(new ViewResultOptions(new { Model = "hello" }), request.BuildControllerContext());
            result.Render();

            Assert.AreEqual("hello", result.View.DataContext);
        }

        [Test]
        public void ShouldSetNavigatorIfINavigationAwareIsImplemented()
        {
            var request = RequestBuilder.CreateRequest();
            var result = new SampleViewEngineResult<ReallySmartView>(new ViewResultOptions(), request.BuildControllerContext());
            result.Render();

            Assert.AreEqual(request.Navigator.Object, result.View.Navigator);
        }
    }
}
