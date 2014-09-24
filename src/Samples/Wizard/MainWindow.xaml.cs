using System;
using System.Windows;
using Magellan.Events;
using Magellan.Progress;

namespace Wizard
{
    public partial class MainWindow : Window, INavigationProgressListener
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void UpdateProgress(NavigationEvent navigationEvent)
        {
            Dispatcher.Invoke(
                new Action(delegate
                {
                    if (navigationEvent is BeginRequestNavigationEvent)
                    {
                        BusyIndicator.IsBusy = true;
                    }
                    if (navigationEvent is CompleteNavigationEvent)
                    {
                        BusyIndicator.IsBusy = false;
                    }
                }));
        }
    }
}
