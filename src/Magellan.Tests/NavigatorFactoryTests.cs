using System;
using System.Windows.Controls;
using Magellan.Abstractions;
using Magellan.Exceptions;
using Magellan.Routing;
using Magellan.Tests.Helpers;
using Magellan.Views;
using Moq;
using NUnit.Framework;

namespace Magellan.Tests
{
    [TestFixture]
    public class NavigatorFactoryTests
    {
        [Test]
        public void ResolverIsRequired()
        {
            Assert.Throws<ArgumentNullException>(() => new NavigatorFactory((IRouteResolver)null));
        }

        [Test]
        public void CanCreateNavigatorForINavigationService()
        {
            var resolver = new Mock<IRouteResolver>();
            var nav = new Mock<INavigationService>();
            var factory = new NavigatorFactory(resolver.Object);
            var navigator = factory.CreateNavigator(nav.Object);
            navigator.ResetHistory();
            nav.Verify(x => x.ResetHistory());
        }

        [Test]
        public void CanCreateNavigatorForFrame()
        {
            var frame = new Frame();
            var resolver = new Mock<IRouteResolver>();
            var factory = new NavigatorFactory(resolver.Object);
            var navigator = factory.CreateNavigator(frame);
            navigator.CanGoBack.ToString();
        }

        [Test]
        public void CanCreateNavigatorForContentFrame()
        {
            var frame = new ContentControl();
            var resolver = new Mock<IRouteResolver>();
            var factory = new NavigatorFactory(resolver.Object);
            var navigator = factory.CreateNavigator(frame);
            navigator.CanGoBack.ToString();
        }

        [Test]
        public void CanCreateNavigatorForASourceElementWhenInATree()
        {
            var frame = new Frame();
            var button = new Button();
            frame.Content = button;

            var window = new TestWindow(frame);
            window.Show();
            window.ProcessEvents();

            var resolver = new Mock<IRouteResolver>();
            var factory = new NavigatorFactory(resolver.Object);
            var navigator = factory.GetOwningNavigator(button);
            navigator.ResetHistory();

            window.Close();
        }

        [Test]
        public void CanCreateNavigatorForASourceElementWhenNavigatorAlreadySet()
        {
            var oldNavigator = new Mock<INavigator>();
            var button = new Button();
            NavigationProperties.SetNavigator(button, oldNavigator.Object);
            
            var resolver = new Mock<IRouteResolver>();
            var factory = new NavigatorFactory(resolver.Object);
            var navigator = factory.GetOwningNavigator(button);
            Assert.AreEqual(navigator, oldNavigator.Object);
        }

        [Test]
        public void SourceElementsNotInATreeWhenNavigatingWillThrow()
        {
            var button = new Button();

            var resolver = new Mock<IRouteResolver>();
            var factory = new NavigatorFactory(resolver.Object);
            var navigator = factory.GetOwningNavigator(button);

            Assert.Throws<ImpossibleNavigationRequestException>(navigator.ResetHistory);
        }

        [Test]
        public void CreatingNavigatorsForCommonUIElementsIsNotAllowed()
        {
            // Since NavigatorFactory.For() can take a ContentControl, it's possible to accidentally use it
            // when NavigatorFactory.ForOwnerOf() was intended. To help this, we throw an exception when 
            // common content controls are used.
            var button = new Button();

            var resolver = new Mock<IRouteResolver>();
            var factory = new NavigatorFactory(resolver.Object);
            Assert.Throws<ArgumentException>(() => factory.CreateNavigator(button));
        }
    }
}
