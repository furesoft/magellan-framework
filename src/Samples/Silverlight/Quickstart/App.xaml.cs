using System.Windows;
using System.Windows.Controls;
using Magellan;
using Magellan.Framework;
using Magellan.Routing;
using Quickstart.Controllers;
using Quickstart.Views.Shared;

namespace Quickstart
{
    public partial class App : Application
    {
        public App()
        {
            Startup += ApplicationStartup;
            UnhandledException += ApplicationUnhandledException;
            InitializeComponent();
        }

        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            var main = new MainView();
            RootVisual = main;

            var controllerFactory = new ControllerFactory();
            controllerFactory.Register("Home", () => new HomeController());

            var routes = new RouteCollection();
            routes.Register("Patient", "Patients/{patientId}", new ControllerRouteHandler(controllerFactory)).Defaults(controller => "Home", action => "Patient");
            routes.Register("Default", "{controller}/{action}", new ControllerRouteHandler(controllerFactory));

            var navigator = new Navigator(routes);
            navigator.RegisterFrame(main.MainFrame);
            navigator.Navigate(controller => "Home", action => "Index");
        }
        
        // Standard Silverlight rubbish for debugging
        private static void ApplicationUnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                e.Handled = true;
                ChildWindow errorWin = new ErrorWindow(e.ExceptionObject);
                errorWin.Show();
            }
        }
    }
}