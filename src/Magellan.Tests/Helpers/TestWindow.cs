using System;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;

namespace Magellan.Tests.Helpers
{
    public class TestWindow : Window, IDisposable
    {
        public TestWindow() : this(null)
        {
        }

        public TestWindow(object child)
        {
            Width = 300;
            Height = 300;
            Content = child;
        }

        public void LoadContent(string content)
        {
            Content = XamlReader.Parse(content);
            ProcessEvents();
        }

        public void ProcessEvents()
        {
            ProcessEvents(30);
        }

        public void ProcessEvents(int milliseconds)
        {
            var frame = new DispatcherFrame(true);
            var dispatcherTimer = new DispatcherTimer(
                TimeSpan.FromMilliseconds(milliseconds),
                DispatcherPriority.Normal,
                (x, y) => { frame.Continue = false; ((DispatcherTimer)x).Stop(); },
                Dispatcher
                );
            dispatcherTimer.Start();
            Dispatcher.PushFrame(frame);
        }

        public void WaitForManualClose()
        {
            var frame = new DispatcherFrame(true);
            Dispatcher.PushFrame(frame);
        }

        public void Dispose()
        {
            if (IsLoaded) Close();
        }
    }
}
