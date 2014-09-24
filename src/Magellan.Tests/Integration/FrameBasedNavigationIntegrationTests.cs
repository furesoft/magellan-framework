using System.Collections.ObjectModel;
using System.Windows.Controls;
using Magellan.Events;
using Magellan.Exceptions;
using Magellan.Framework;
using Magellan.Progress;
using Magellan.Tests.Helpers;
using Moq;
using NUnit.Framework;

namespace Magellan.Tests.Integration
{
    public class View1 : Page { }
    public class View2 : Page { }

    [TestFixture]
    public class FrameBasedNavigationIntegrationTests : UITestBase
    {
        #region SUT

        public class SampleController : Controller
        {
            public ActionResult Action1() { return Page("View1"); }
            public ActionResult Action2() { return Page("View2"); }
            public ActionResult Action3() { return Page(); }
            public ActionResult ActionGoBack() { return Back(); }
            public ActionResult ActionReturn() { return Back(true); }
            public ActionResult ActionRedirect() { return Redirect("Action2"); }
            public ActionResult Action1AndReset() { return Page("View1").ClearNavigationHistory(); }

            public ActionResult ActionWithModel()
            {
                return Page("View2", "Hello");
            }
            public ActionResult ActionWithModel(string value)
            {
                return Page("View2", value);
            }
        }

        #endregion

        public Frame Frame { get; set; }
        public INavigatorFactory NavigatorFactory { get; set; }
        public INavigator Navigator { get; set; }

        protected override void AfterSetup()
        {
            Window.Content = Frame = new Frame();

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
            Assert.IsInstanceOf<View1>(Navigator.Content);

            Navigator.Navigate("foobar/Action2");
            ProcessEvents();
            Assert.IsInstanceOf<View2>(Navigator.Content);
        }

        [Test]
        public void ControllersThatThrowShouldCancelNavigation()
        {
            Navigator.Navigate("foobar/Action2");
            ProcessEvents();
            Assert.IsInstanceOf<View2>(Navigator.Content);

            Assert.Throws<ViewNotFoundException>(() => Navigator.Navigate("foobar/Action3"));

            ProcessEvents();
            Assert.IsInstanceOf<View2>(Navigator.Content);
        }

        [Test]
        public void BackResultNavigatesBack()
        {
            Navigator.Navigate("foobar/Action1");
            ProcessEvents();

            Navigator.Navigate("foobar/Action2");
            ProcessEvents();

            Navigator.Navigate("foobar/ActionGoBack");
            ProcessEvents();

            Assert.IsInstanceOf<View1>(Navigator.Content);
            Assert.IsTrue(Navigator.CanGoForward);
        }

        [Test]
        public void RedirectSendsToRedirectedPage()
        {
            Navigator.Navigate("foobar/Action1");
            ProcessEvents();

            Navigator.Navigate("foobar/ActionRedirect");
            ProcessEvents();

            Assert.IsInstanceOf<View2>(Navigator.Content);
        }

        [Test]
        public void ReturnResultNavigatesBackAndRemovesFromJournal()
        {
            Navigator.Navigate("foobar/Action1");
            ProcessEvents();

            Navigator.Navigate("foobar/Action2");
            ProcessEvents();

            Navigator.Navigate("foobar/ActionReturn");
            ProcessEvents();

            Assert.IsInstanceOf<View1>(Navigator.Content);
            Assert.IsFalse(Navigator.CanGoForward);
        }

        [Test]
        public void BackAndForwardNavigationShouldWorkForMvcNavigation()
        {
            Navigator.Navigate("foobar/Action1");
            ProcessEvents();
            Navigator.Navigate("foobar/Action2");
            ProcessEvents();

            Navigator.GoBack();
            ProcessEvents();
            Assert.IsInstanceOf<View1>(Navigator.Content);

            Navigator.GoForward();
            ProcessEvents();
            Assert.IsInstanceOf<View2>(Navigator.Content);
        }

        [Test]
        public void ModelsShouldBePassedToViews()
        {
            Navigator.Navigate("foobar/ActionWithModel");
            ProcessEvents();
            Assert.IsInstanceOf<View2>(Navigator.Content);
            Assert.AreEqual("Hello", ((View2)Navigator.Content).DataContext);
        }

        [Test]
        public void BackAndForwardNavigationShouldWorkForRegularNavigation()
        {
            Navigator.Navigate("foobar/ActionWithModel");
            ProcessEvents();
            Assert.IsInstanceOf<View2>(Navigator.Content);
            Navigator.NavigateDirectToContent(new Button(), "foo");
            ProcessEvents();
            Navigator.GoBack();
            ProcessEvents();
            Assert.IsInstanceOf<View2>(Navigator.Content);
            Assert.IsFalse(Navigator.CanGoBack);
        }

        [Test]
        public void ResetNavigationHistory()
        {
            Navigator.Navigate("foobar/Action1");
            ProcessEvents();
            Assert.IsInstanceOf<View1>(Navigator.Content);

            Navigator.Navigate("foobar/Action2");
            ProcessEvents();
            Assert.IsInstanceOf<View2>(Navigator.Content);
            Assert.IsTrue(Navigator.CanGoBack);

            Navigator.Navigate("foobar/Action1AndReset");
            ProcessEvents();
            Assert.IsInstanceOf<View1>(Navigator.Content);
            Assert.IsFalse(Navigator.CanGoBack);
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
