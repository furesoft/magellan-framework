using System;
using System.Collections.Generic;
using System.IO;
using Magellan.Framework;
using Magellan.Progress;
using Magellan.Routing;
using Magellan.Tests.Helpers.TypeGeneration;
using Moq;
using NUnit.Framework;

namespace Magellan.Tests.Framework
{
    public abstract class ViewEngineTestBase
    {
        private readonly Func<IViewEngine> _viewEngineCreator;

        protected ViewEngineTestBase(Func<IViewEngine> viewEngineCreator)
        {
            _viewEngineCreator = viewEngineCreator;
        }

        [SetUp]
        public void SetUp()
        {
            Project = new DynamicProject();
        }

        protected DynamicProject Project { get; set; }

        protected ViewEngineResult FindViewForController(string controllerName, string viewName)
        {
            return FindViewForController(controllerName, viewName, null);
        }

        protected ViewEngineResult FindViewForController(string controllerName, string viewName, object viewParameters)
        {
            var controllerType = Project.Assembly.GetType(controllerName, true);
            var controllerShortName = Path.GetExtension(controllerName.Replace("Controller", "")).Substring(1);

            var context = new ControllerContext(Activator.CreateInstance(controllerType) as IController, 
                new ResolvedNavigationRequest(
                    new Uri("magellan://foobar"),
                    "foobar",
                    true,
                    new Mock<INavigator>().Object, 
                    new Mock<IRoute>().Object, 
                    new RouteValueDictionary(new{controller = controllerShortName, action = "x"}), 
                    new List<INavigationProgressListener>()
                    ), 
                    ViewEngines.CreateDefaults());
            var viewEngine = _viewEngineCreator();
            
            return viewEngine.FindView(context, new ViewResultOptions(viewParameters), viewName);
        }
    }
}