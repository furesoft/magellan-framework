using System.Windows;
using Magellan;
using Magellan.Framework;
using TaxCalculator.Features.Home;
using TaxCalculator.Features.Tax;
using TaxCalculator.Features.Tax.Model;

namespace TaxCalculator
{
    /// <summary>
    /// Main entry point for the application.
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Tax settings
            var taxes = new TaxEstimatorSelector();
            taxes.AddTaxRate(
                TaxPeriod.FY2009, 
                new TaxEstimator(
                    new TaxBracketSelector(
                        new TaxBracket(-1, 6000M, 0, 0),
                        new TaxBracket(6000M, 35000M, 0, 0.15M),
                        new TaxBracket(35000M, 80000M, 4350, 0.30M),
                        new TaxBracket(80000M, 180000M, 17850, 0.38M),
                        new TaxBracket(180000M, decimal.MaxValue, 55850, 0.45M)
                        ),
                    new MedicareLevy(70000, 0.015M))
                );
            taxes.AddTaxRate(
                TaxPeriod.FY2010,
                new TaxEstimator(
                    new TaxBracketSelector(
                        new TaxBracket(-1, 6000M, 0, 0),
                        new TaxBracket(6000M, 37000M, 0, 0.15M),
                        new TaxBracket(37000M, 80000M, 4650, 0.30M),
                        new TaxBracket(80000M, 180000M, 17550, 0.37M),
                        new TaxBracket(180000M, decimal.MaxValue, 54550, 0.45M)
                        ), 
                    new MedicareLevy(70000, 0.015M))
                );

            // Configure Magellan
            var controllerFactory = new AsyncControllerFactory();
            controllerFactory.Register("Home", () => new HomeController());
            controllerFactory.Register("Tax", () => new TaxController(taxes));

            var routes = new ControllerRouteCatalog(controllerFactory);
            routes.MapRoute("{controller}/{action}/{id}", new {controller = "Home", action = "Index", id = ""});

            var factory = new NavigatorFactory("tax", routes);
            var mainWindow = new MainWindow(factory);
            mainWindow.MainNavigator.Navigate<HomeController>(x => x.Index());
            mainWindow.Show();
        }
    }
}
