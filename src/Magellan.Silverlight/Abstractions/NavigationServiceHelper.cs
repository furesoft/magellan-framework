using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Magellan.Abstractions
{
    public static class NavigationServiceHelper
    {
        public static Frame GetFrame(DependencyObject element)
        {
            var current = element;
            while (current != null)
            {
                if (current is Frame)
                {
                    return (Frame) current;
                }

                current = VisualTreeHelper.GetParent(current);
            }
            return null;
        }
    }
}