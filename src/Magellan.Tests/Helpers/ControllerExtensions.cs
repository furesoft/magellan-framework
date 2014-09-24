using Magellan.Framework;

namespace Magellan.Tests.Helpers
{
    public static class ControllerExtensions
    {
        public static void Execute(this IController controller, string action)
        {
            var request = RequestBuilder.CreateRequest(controller.GetType().Name, action, null);
            controller.Execute(
                new ControllerContext(
                    controller,
                    request.BuildRequest(),
                    ViewEngines.CreateDefaults(), 
                    ModelBinders.CreateDefaults(),
                    () => { }
                ));
        }

        public static void Execute(this IController controller, string action, object parameters)
        {
            var request = RequestBuilder.CreateRequest(controller.GetType().Name, action, parameters);
            controller.Execute(
                new ControllerContext(
                    controller,
                    request.BuildRequest(),
                    ViewEngines.CreateDefaults(), 
                    ModelBinders.CreateDefaults(),
                    () => {}
                ));
        }
    }
}
