using System.Windows;
using Magellan;
using Magellan.Framework;
using Sample.Features.Details;
using Sample.Features.Details.ServiceProxies;
using Sample.Features.Search;
using Sample.Features.Search.ServiceProxies;

namespace Sample
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var views = new ViewModelFactory();
            views.Register("Search", () => new SearchView(), () => new SearchViewModel(new SearchService()));
            views.Register("Details", () => new DetailsView(), () => new DetailsViewModel(new DetailsService()));

            var routes = new ViewModelRouteCatalog(views);
            routes.MapRoute("views/details/{id}", new { viewModel = "Details" });
            routes.MapRoute("views/{viewModel}");

            var shell = new ShellWindow();
            var navigation = new NavigatorFactory(routes);
            var navigator = navigation.CreateNavigator(shell.MainContent);
            shell.Show();

            navigator.Navigate("views/Search");
        }
    }
}
