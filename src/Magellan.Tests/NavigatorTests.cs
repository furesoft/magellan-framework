using System;
using System.Data;
using Magellan.Abstractions;
using Magellan.Exceptions;
using Magellan.Framework;
using Magellan.Routing;
using Moq;
using NUnit.Framework;

namespace Magellan.Tests
{
    [TestFixture]
    public class NavigatorTests
    {
        protected Mock<INavigatorFactory> Factory { get; set; }
        protected Mock<IRouteResolver> Resolver { get; set; }
        protected Mock<INavigationService> NavigationService { get; set; }

        public NavigatorTests()
        {
            Factory = new Mock<INavigatorFactory>();
            Resolver = new Mock<IRouteResolver>();
            NavigationService = new Mock<INavigationService>();
        }

        [Test]
        public void FactoryProvidesAccessToCreator()
        {
            var navigator = new Navigator(Factory.Object, null, "magellan", Resolver.Object, () => NavigationService.Object);
            Assert.AreEqual(navigator.Factory, Factory.Object);
        }

        [Test]
        public void NavigatingToUnrecognizedRouteThrows()
        {
            Resolver.Setup(x => x.MatchRouteToPath(It.IsAny<RouteValueDictionary>())).Returns(PathMatch.Failure(null, "Boogedyboo"));

			var navigator = new Navigator(Factory.Object, null, "magellan", Resolver.Object, () => NavigationService.Object);
            Assert.Throws<UnroutableRequestException>(() => navigator.Navigate(new { Foo = "bar"}));
        }

        [Test]
        public void NavigatingToUnrecognizedPathThrows()
        {
            Resolver.Setup(x => x.MatchPathToRoute(It.IsAny<string>())).Returns(RouteMatch.Failure(null, "Boogedyboo"));

			var navigator = new Navigator(Factory.Object, null, "magellan", Resolver.Object, () => NavigationService.Object);
            Assert.Throws<UnroutableRequestException>(() => navigator.Navigate(new Uri("magellan://patients/list")));
        }

        [Test]
        public void NavigateToPathResolvesRightController()
        {
            var controller1 = new Mock<IController>();
            var controller2 = new Mock<IController>();
            var controllers = new ControllerFactory();
            controllers.Register("C1", () => controller1.Object);
            controllers.Register("C2", () => controller2.Object);
            var routes = new ControllerRouteCatalog(controllers);
            routes.MapRoute("search/{action}", new { controller = "C1" });
            routes.MapRoute("patients/{action}", new { controller = "C2"});

			var navigator = new Navigator(Factory.Object, null, "magellan", new RouteResolver(routes), () => NavigationService.Object);

            navigator.Navigate(new Uri("magellan://patients/list"));
            controller2.Verify(x => x.Execute(It.IsAny<ControllerContext>()));

            navigator.Navigate(new Uri("magellan://search/list"));
            controller1.Verify(x => x.Execute(It.IsAny<ControllerContext>()));
        }

        [Test]
        public void NavigateToRouteResolvesRightController()
        {
            var controller1 = new Mock<IController>();
            var controller2 = new Mock<IController>();
            var controllers = new ControllerFactory();
            controllers.Register("C1", () => controller1.Object);
            controllers.Register("C2", () => controller2.Object);
            var routes = new ControllerRouteCatalog(controllers);
            routes.MapRoute("search/{action}", new { controller = "C1" });
            routes.MapRoute("patients/{action}", new { controller = "C2" });

			var navigator = new Navigator(Factory.Object, null, "magellan", new RouteResolver(routes), () => NavigationService.Object);

            navigator.Navigate(new { controller = "C2", action = "foo" });
            controller2.Verify(x => x.Execute(It.IsAny<ControllerContext>()));

            navigator.Navigate(new { controller = "C1", action = "foo" });
            controller1.Verify(x => x.Execute(It.IsAny<ControllerContext>()));
        }

        [Test]
        public void StringableRequestsDoNotHaveAdditionalUriData()
        {
            var controller1 = new Mock<IController>();
            var controller2 = new Mock<IController>();
            var controllers = new ControllerFactory();
            controllers.Register("C1", () => controller1.Object);
            controllers.Register("C2", () => controller2.Object);
            var routes = new ControllerRouteCatalog(controllers);
            routes.MapRoute("search/{action}", new { controller = "C2" });

			var navigator = new Navigator(Factory.Object, null, "magellan", new RouteResolver(routes), () => NavigationService.Object);

            navigator.Navigate(new { controller = "C2", action = "foo" });
            controller2.Verify(x => x.Execute(It.Is<ControllerContext>(r => r.Request.HasNonUriData == false && r.Request.Uri.ToString() == "magellan://search/foo")));

            navigator.Navigate(new { controller = "C2", action = "foo", abc = "123" });
            controller2.Verify(x => x.Execute(It.Is<ControllerContext>(r => r.Request.HasNonUriData == false && r.Request.Uri.ToString() == "magellan://search/foo?abc=123")));
        }

        [Test]
        public void NonStringableRequestsHaveAdditionalUriData()
        {
            var controller1 = new Mock<IController>();
            var controller2 = new Mock<IController>();
            var controllers = new ControllerFactory();
            controllers.Register("C1", () => controller1.Object);
            controllers.Register("C2", () => controller2.Object);
            var routes = new ControllerRouteCatalog(controllers);
            routes.MapRoute("search/{action}", new { controller = "C2" });

			var navigator = new Navigator(Factory.Object, null, "magellan", new RouteResolver(routes), () => NavigationService.Object);

            navigator.Navigate(new { controller = "C2", action = "foo", abc = "123", def = new DataSet() });
            controller2.Verify(x => x.Execute(It.Is<ControllerContext>(r => r.Request.HasNonUriData && r.Request.Uri.ToString() == "magellan://search/foo?abc=123")));
        }
    }
}