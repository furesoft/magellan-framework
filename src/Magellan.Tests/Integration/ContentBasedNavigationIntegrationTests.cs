using System.Collections.ObjectModel;
using System.Windows.Controls;
using Magellan.Events;
using Magellan.Framework;
using Magellan.Progress;
using Magellan.Tests.Helpers;
using Moq;
using NUnit.Framework;

namespace Magellan.Tests.Integration
{
    public class ContentView1 : ContentControl { }
    public class ContentView2 : ContentControl { }

    [TestFixture]
    public class ContentBasedNavigationIntegrationTests : UITestBase
    {
        #region SUT

        public class SampleController : Controller
        {
            public ActionResult Action1() { return Page("ContentView1"); }
            public ActionResult Action2() { return Page("ContentView2"); }
            public ActionResult Action3() { return Page(); }
            public ActionResult ActionGoBack() { return Back(); }
            public ActionResult ActionReturn() { return Back(true); }
            public ActionResult ActionRedirect() { return Redirect("Action2"); }
            public ActionResult Action1AndReset() { return Page("ContentView1").ClearNavigationHistory(); }

            public ActionResult ActionWithModel()
            {
                return Page("ContentView2", "Hello");
            }
            public ActionResult ActionWithModel(string value)
            {
                return Page("ContentView2", value);
            }
        }

        #endregion

        public ContentControl Frame { get; set; }
        public INavigatorFactory NavigatorFactory { get; set; }
        public INavigator Navigator { get; set; }

        protected override void AfterSetup()
        {
            Window.Content = Frame = new ContentControl();

            var controllerFactory = new ControllerFactory();
            controllerFactory.Register("Sample", () => new SampleController() { ActionInvoker = new DefaultActionInvoker() });

            var routes = new ControllerRouteCatalog(controllerFactory);
            routes.MapRoute("foobar/{action}", new {controller = "Sample"});

            NavigatorFactory = new NavigatorFactory(routes);
            Navigator = NavigatorFactory.CreateNavigator(Frame);
        }

        [Test]
        public void WhenStartingBackAndForwardShouldBeDisabled()
        {
            Assert.IsFalse(Navigator.CanGoBack);
            Assert.IsFalse(Navigator.CanGoForward);
        }

        [Test]
        public void ShouldBeAbleToNavigateBetweenViews()
        {
            Navigator.Navigate("foobar/Action1");
            ProcessEvents();
            Assert.IsInstanceOf<ContentView1>(Navigator.Content);

            Navigator.Navigate("foobar/Action2");
            ProcessEvents();
            Assert.IsInstanceOf<ContentView2>(Navigator.Content);
        }

        [Test]
        public void HistoryRelatedActionsAreNoOps()
        {
            Navigator.Navigate("foobar/Action1");
            ProcessEvents();
            Assert.IsInstanceOf<ContentView1>(Navigator.Content);

            Navigator.Navigate("foobar/Action2");
            ProcessEvents();
            Assert.IsInstanceOf<ContentView2>(Navigator.Content);

            Navigator.Navigate("foobar/ActionGoBack");
            ProcessEvents();
            Assert.IsInstanceOf<ContentView2>(Navigator.Content);

            Navigator.Navigate("foobar/ActionReturn");
            ProcessEvents();
            Assert.IsInstanceOf<ContentView2>(Navigator.Content);
        }

        [Test]
        public void AfterNavigationBackAndForwardShouldStillBeDisabled()
        {
            Navigator.Navigate("foobar/Action1");
            ProcessEvents();
            Assert.IsInstanceOf<ContentView1>(Navigator.Content);

            Assert.IsFalse(Navigator.CanGoBack);
            Assert.IsFalse(Navigator.CanGoForward);
        }

        [Test]
        public void ProgressListenersShouldBeNotified()
        {
            var updates = new Collection<NavigationEvent>();
            var listener = new Mock<INavigationProgressListener>();
            listener.Setup(x => x.UpdateProgress(It.IsAny<NavigationEvent>())).Callback(
                delegate(NavigationEvent requst)
                {
                    updates.Add(requst);
                });

            NavigatorFactory.ProgressListeners.Add(listener.Object);

            Navigator.Navigate("foobar/Action1");
            ProcessEvents();

            Assert.AreEqual(10, updates.Count);
            Assert.IsAssignableFrom<BeginRequestNavigationEvent>(updates[0]);
            Assert.IsAssignableFrom<ResolvingControllerNavigationEvent>(updates[1]);
            Assert.IsAssignableFrom<ResolvingActionNavigationEvent>(updates[2]);
            Assert.IsAssignableFrom<PreActionFiltersNavigationEvent>(updates[3]);
            Assert.IsAssignableFrom<ExecutingActionNavigationEvent>(updates[4]);
            Assert.IsAssignableFrom<PostActionFiltersNavigationEvent>(updates[5]);
            Assert.IsAssignableFrom<PreResultFiltersNavigationEvent>(updates[6]);
            Assert.IsAssignableFrom<ExecutingResultNavigationEvent>(updates[7]);
            Assert.IsAssignableFrom<PostResultFiltersNavigationEvent>(updates[8]);
            Assert.IsAssignableFrom<CompleteNavigationEvent>(updates[9]);
        }
    }
}
