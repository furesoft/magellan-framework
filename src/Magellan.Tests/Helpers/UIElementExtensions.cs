using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace Magellan.Tests.Helpers
{
    public static class UIElementExtensions
    {
        public static T Find<T>(this FrameworkElement element, string name)
        {
            var result = element.FindName(name)
                ?? ((element is ContentControl) ? ((FrameworkElement)((ContentControl)element).Content).FindName(name) : null);
            return (T) result;
        }

        public static void ExecuteClick(this Button button)
        {
            var peer = new ButtonAutomationPeer(button);
            var invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
            if (invokeProv != null)
            {
                invokeProv.Invoke();
            }
        }
    }
}
