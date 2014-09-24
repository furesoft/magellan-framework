using System;
using System.Windows.Navigation;

namespace Magellan.Abstractions
{
    public class NavigationServiceDecorator : INavigationService
    {
        private readonly Func<INavigationService> _navigationServiceResolver;
        private INavigationService _navigationService;

        public NavigationServiceDecorator(Func<INavigationService> navigationService)
        {
            _navigationServiceResolver = navigationService;
        }

        private INavigationService GetNavigationService()
        {
            if (_navigationService == null)
            {
                _navigationService = _navigationServiceResolver();
            }
            return _navigationService;
        }

        public event NavigatingCancelEventHandler Navigating;
        public event NavigatedEventHandler Navigated;
        public event NavigationFailedEventHandler NavigationFailed;

        public bool CanGoBack
        {
            get { return GetNavigationService().CanGoBack; }
        }

        public bool CanGoForward
        {
            get { return GetNavigationService().CanGoForward; }
        }

        public void GoBack()
        {
            GetNavigationService().GoBack();
        }

        public object Content
        {
            get { return GetNavigationService().Content; }
        }

        public void GoForward()
        {
            GetNavigationService().GoForward();
        }

        public bool NavigateDirectToContent(object content)
        {
            return GetNavigationService().NavigateDirectToContent(content);
        }

        public bool NavigateDirectToContent(object content, object customState)
        {
            return GetNavigationService().NavigateDirectToContent(content, customState);
        }

        public IDispatcher Dispatcher
        {
            get { return GetNavigationService().Dispatcher; }
        }
    }
}
