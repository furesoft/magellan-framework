using System.Windows;
using System.Windows.Input;

namespace Magellan.Utilities
{
    internal static class UIElementExtensions
    {
        public static void ToggleSuppressInput(this UIElement element, bool isDisabled)
        {
            if (isDisabled)
            {
                element.PreviewKeyDown += IgnoreKeys;
                element.PreviewKeyUp += IgnoreKeys;
                element.PreviewTextInput += IgnoreText;
                element.PreviewMouseMove += IgnoreMouse;
                element.PreviewMouseUp += IgnoreMouse;
                element.PreviewMouseDown += IgnoreMouse;
            }
            else
            {
                element.PreviewKeyDown -= IgnoreKeys;
                element.PreviewKeyUp -= IgnoreKeys;
                element.PreviewTextInput -= IgnoreText;
                element.PreviewMouseMove -= IgnoreMouse;
                element.PreviewMouseUp -= IgnoreMouse;
                element.PreviewMouseDown -= IgnoreMouse;
            }
        }

        private static void IgnoreMouse(object sender, MouseEventArgs e)
        {
            e.Handled = true;
        }

        private static void IgnoreText(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true;
        }

        static void IgnoreKeys(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
    }
}