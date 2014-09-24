using System.Windows;
using System.Windows.Controls;

namespace iPhone.Controls
{
    public class BusyIndicator : Control
    {
        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register("IsBusy", typeof(bool), typeof(BusyIndicator), new UIPropertyMetadata(false));

        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }
    }
}
