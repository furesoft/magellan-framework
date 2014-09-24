using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Magellan.Tests.Helpers;
using Magellan.Utilities;
using NUnit.Framework;

namespace Magellan.Tests.Utilities
{
    [TestFixture]
    public class UIElementExtensionsTests
    {
        [Test]
        public void ToggleSuppressInputTest()
        {
            var button = new Button();
            var window = new TestWindow(button);
            window.Show();
            
            CheckMouseEvent(window, button, UIElement.PreviewMouseUpEvent, false);
            CheckMouseEvent(window, button, UIElement.PreviewMouseDownEvent, false);
            
            window.ToggleSuppressInput(true);

            CheckMouseEvent(window, button, UIElement.PreviewMouseUpEvent, true);
            CheckMouseEvent(window, button, UIElement.PreviewMouseDownEvent, true);
            
            window.ToggleSuppressInput(false);

            CheckMouseEvent(window, button, UIElement.PreviewMouseUpEvent, false);
            CheckMouseEvent(window, button, UIElement.PreviewMouseDownEvent, false);
            
            window.Close();
        }

        private static void CheckMouseEvent(TestWindow window, IInputElement button, RoutedEvent routedEvent, bool shouldBeHandled)
        {
            var args = new MouseButtonEventArgs(Mouse.PrimaryDevice, (int) DateTime.Now.Ticks, MouseButton.Left);
            args.RoutedEvent = routedEvent;
            button.RaiseEvent(args);
            window.ProcessEvents();
            Assert.AreEqual(shouldBeHandled, args.Handled);
        }
    }
}
