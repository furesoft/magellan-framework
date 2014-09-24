using System;
using System.Windows;
using System.Windows.Input;
using Magellan;
using Magellan.Events;
using Magellan.Progress;
using Magellan.Transitionals;

namespace iPhone
{
    public partial class MainWindow : Window, INavigationProgressListener
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += (x, y) => Navigator.NavigateWithTransition("Home", "Home", "ZoomIn");
        }

        public INavigator Navigator { get; set; }

        private void HomeButtonClicked(object sender, RoutedEventArgs e)
        {
            Navigator.NavigateWithTransition("Home", "Home", "ZoomOut");
        }

        private void BeginMove(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void UpdateProgress(NavigationEvent navigationEvent)
        {
            Dispatcher.Invoke(
                new Action(delegate
                {
                    if (navigationEvent is BeginRequestNavigationEvent) BusyIndicator.IsBusy = true;
                    if (navigationEvent is CompleteNavigationEvent) BusyIndicator.IsBusy = false;
                }));
        }
    }
}
