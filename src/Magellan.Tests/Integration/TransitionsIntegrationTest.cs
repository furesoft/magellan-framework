using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using Magellan.Abstractions;
using Magellan.Tests.Helpers;
using Magellan.Transitionals;
using Magellan.Transitionals.Transitions;
using NUnit.Framework;

namespace Magellan.Tests.Integration
{
    [TestFixture]
    public class TransitionsIntegrationTest : UITestBase
    {
        protected Frame Frame { get; set; }

        protected override void AfterSetup()
        {
            NavigationTransitions.Table.Add("Back", "Forward", () => new SlideTransition(SlideDirection.Back));
            NavigationTransitions.Table.Add("Forward", "Back", () => new SlideTransition(SlideDirection.Forward));
            NavigationTransitions.Table.Add("ZoomIn", "ZoomOut", () => new ZoomInTransition());
            NavigationTransitions.Table.Add("ZoomOut", "ZoomIn", () => new ZoomOutTransition());

            const string xaml = @"<ControlTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' 
	                xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'
                    xmlns:magellan='clr-namespace:Magellan.Transitionals;assembly=Magellan.Transitionals'
                    TargetType='{x:Type Frame}'
                    >
                    <magellan:NavigationTransitionPresenter x:Name='transitioner' Content='{TemplateBinding Content}' />
                </ControlTemplate>";

            Window.Content = Frame = new Frame();
            ProcessEvents();
            
            Frame.Template = (ControlTemplate)XamlReader.Parse(xaml, new ParserContext());
            ProcessEvents();
            var transitionPresenter = (NavigationTransitionPresenter)VisualTreeHelper.GetChild(Frame, 0);
            TransitionSelector = (NavigationTransitionSelector)transitionPresenter.TransitionSelector;
        }

        private NavigationTransitionSelector TransitionSelector { get; set; }

        [Test]
        public void NavigateWithTransitions()
        {
            var navigator = new FrameNavigationServiceWrapper(Frame.Dispatcher, Frame);

            navigator.NavigateDirectToContent(new Button());
            Assert.IsNull(TransitionSelector.CurrentTransition);
            ProcessEventsSlow();

            navigator.NavigateDirectToContent(new Button(), "ZoomIn");
            Assert.AreEqual(TransitionSelector.CurrentTransition.Name, "ZoomIn");
            ProcessEventsSlow();

            navigator.NavigateDirectToContent(new Button(), "Forward");
            Assert.AreEqual(TransitionSelector.CurrentTransition.Name, "Forward");
            ProcessEventsSlow();

            navigator.GoBack();
            Assert.AreEqual(TransitionSelector.CurrentTransition.Name, "Back");
            ProcessEventsSlow();

            navigator.GoForward();
            Assert.AreEqual(TransitionSelector.CurrentTransition.Name, "Forward");
            ProcessEventsSlow();

            navigator.GoBack();
            Assert.AreEqual(TransitionSelector.CurrentTransition.Name, "Back");
            ProcessEventsSlow();

            navigator.GoBack();
            Assert.AreEqual(TransitionSelector.CurrentTransition.Name, "ZoomOut");
            ProcessEventsSlow();
        }
    }
}
