using System.ComponentModel;
using System.Windows;

namespace Magellan.Behaviors
{
    /// <summary>
    /// A Blend Trigger Action that makes it easy to navigate using Magellan controllers.
    /// </summary>
    public class NavigateControllerAction : NavigateAction
    {
        /// <summary>
        /// Dependency property for the Controller property.
        /// </summary>
        public static readonly DependencyProperty ControllerProperty = DependencyProperty.Register("Controller", typeof(string), typeof(NavigateControllerAction), new UIPropertyMetadata(string.Empty));

        /// <summary>
        /// Dependency property for the Action property.
        /// </summary>
        public static readonly DependencyProperty ActionProperty = DependencyProperty.Register("Action", typeof(string), typeof(NavigateControllerAction), new UIPropertyMetadata(string.Empty));

        /// <summary>
        /// Gets or sets the name of the controller that you wish to navigate to.
        /// </summary>
        /// <value>The controller.</value>
        [Category("Navigation")]
        [Description("The name of the controller that you wish to navigate to.")]
        public string Controller
        {
            get { return (string)GetValue(ControllerProperty); }
            set { SetValue(ControllerProperty, value); }
        }

        /// <summary>
        /// Gets or sets the name of the action on the controller that you wish to navigate to.
        /// </summary>
        /// <value>The action.</value>
        [Category("Navigation")]
        [Description("The name of the action on the controller that you wish to navigate to.")]
        public string Action
        {
            get { return (string)GetValue(ActionProperty); }
            set { SetValue(ActionProperty, value); }
        }

        protected override void PrepareRequest(Routing.RouteValueDictionary request)
        {
            base.PrepareRequest(request);
            request["controller"] = Controller;
            request["action"] = Action;
        }
    }
}