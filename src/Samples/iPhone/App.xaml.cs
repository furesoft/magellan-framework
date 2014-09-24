using System;
using System.Security.Principal;
using System.Windows;
using iPhone.Applications.Home.Controllers;
using iPhone.Applications.Mail.Controllers;
using iPhone.Applications.Phone.Controllers;
using iPhone.Applications.Phone.Repositories.InMemory;
using iPhone.Applications.Settings.Controllers;
using Magellan;
using Magellan.Framework;
using Magellan.Transitionals;
using Magellan.Transitionals.Transitions;

namespace iPhone
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);

            // Setup controllers
            var controllerFactory = new AsyncControllerFactory();
            controllerFactory.Register("Home", () => new HomeController());
            controllerFactory.Register("Phone", () => new PhoneController(new InMemoryContactRepository()));
            controllerFactory.Register("Mail", () => new MailController());
            controllerFactory.Register("Settings", () => new SettingsController());
            
            // Setup transitions
            NavigationTransitions.Table.Add("Back", "Forward", () => new SlideTransition(SlideDirection.Back));
            NavigationTransitions.Table.Add("Forward", "Back", () => new SlideTransition(SlideDirection.Forward));
            NavigationTransitions.Table.Add("ZoomIn", "ZoomOut", () => new ZoomInTransition());
            NavigationTransitions.Table.Add("ZoomOut", "ZoomIn", () => new ZoomOutTransition());

            // Setup routes
            var routes = new ControllerRouteCatalog(controllerFactory);
            routes.MapRoute("{controller}/{action}");

            // Show the main window
            var main = new MainWindow();

            var navigation = new NavigatorFactory(routes);
            var navigator = navigation.CreateNavigator(main.Frame);
            main.Navigator = navigator;

            main.Show();

            base.OnStartup(e);
        }
    }
}
