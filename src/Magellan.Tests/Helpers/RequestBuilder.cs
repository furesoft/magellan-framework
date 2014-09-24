using System;
using System.Collections.Generic;
using Magellan.Abstractions;
using Magellan.Framework;
using Magellan.Progress;
using Magellan.Routing;
using Magellan.Testability;
using Moq;

namespace Magellan.Tests.Helpers
{
    public class RequestBuilder
    {
        private readonly string _controllerName;
        private readonly string _actionName;
        private readonly RouteValueDictionary _routeValues;

        private RequestBuilder(string controllerName, string actionName, object routeValues)
        {
            _controllerName = controllerName;
            _actionName = actionName;
            _routeValues = new RouteValueDictionary(routeValues);
            NavigationService = new Mock<INavigationService>();
            Navigator = new Mock<INavigator>();
            Route = new Mock<IRoute>();
            Controller = new Mock<IController>();
            Path = "TestPath";
            ProgressListeners = new List<INavigationProgressListener>();
            Dispatcher = new SingleThreadDispatcher();
        }

        public static RequestBuilder CreateRequest()
        {
            return new RequestBuilder("MyController", "MyAction", new RouteValueDictionary());
        }

        public static RequestBuilder CreateRequest(string controllerName, string actionName, object routeValues)
        {
            return new RequestBuilder(controllerName, actionName, routeValues);
        }

        public Mock<INavigationService> NavigationService { get; set; }
        public Mock<IController> Controller { get; set; }
        public Mock<INavigator> Navigator { get; set; }
        public Mock<IRoute> Route { get; set; }
        public string Path { get; set; }
        public List<INavigationProgressListener> ProgressListeners { get; set; }
        public IDispatcher Dispatcher { get; set; }

        public ResolvedNavigationRequest BuildRequest()
        {
            Navigator.SetupGet(x => x.Dispatcher).Returns(Dispatcher);

            var values = new RouteValueDictionary(_routeValues);
            values["controller"] = _controllerName;
            values["action"] = _actionName;
            return new ResolvedNavigationRequest(
                new Uri("magellan://MyPath"),
                "MyPath",
                true,
                Navigator.Object, 
                Route.Object, 
                values, 
                ProgressListeners
                );
        }

        public ControllerContext BuildControllerContext()
        {
            return BuildControllerContext(Controller.Object);
        }

        public ControllerContext BuildControllerContext(IController controllerInstance)
        {
            return new ControllerContext(controllerInstance, BuildRequest(), ViewEngines.CreateDefaults(), ModelBinders.CreateDefaults(), () => { });
        }
    }
}
