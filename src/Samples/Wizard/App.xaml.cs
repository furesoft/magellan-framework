using System.Diagnostics;
using System.Windows;
using Magellan;
using Magellan.Framework;
using Wizard.Controllers;

namespace Wizard
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var controllers = new AsyncControllerFactory();
            controllers.Register("Wizard", () => new WizardController());

            var routes = new ControllerRouteCatalog(controllers);
            routes.MapRoute("wizard/{action}", new { controller = "Wizard"});
            
            var navigation = new NavigatorFactory(routes);

            var main = new MainWindow();
            navigation.ProgressListeners.Add(main);
            main.Show();

            var navigator = navigation.CreateNavigator(main.MainFrame);
            navigator.Navigate<WizardController>(x => x.Welcome());

            DispatcherUnhandledException += OnDispatcherUnhandledException;
            base.OnStartup(e);

            Magellan.Diagnostics.TraceSources.MagellanSource.Switch.Level = SourceLevels.Verbose;
        }

        static void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Unhandled exception: " + e.Exception.Message);
            e.Handled = true;
        }
    }
}
