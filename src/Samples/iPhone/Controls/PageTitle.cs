using System.Windows;
using System.Windows.Controls;

namespace iPhone.Controls
{
    public class PageTitle : Control
    {
        public static readonly DependencyProperty HasReturnProperty = DependencyProperty.Register("HasReturn", typeof(bool), typeof(PageTitle), new UIPropertyMetadata(false));
        public static readonly DependencyProperty ReturnTextProperty = DependencyProperty.Register("ReturnText", typeof(string), typeof(PageTitle), new UIPropertyMetadata(string.Empty));
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(PageTitle), new UIPropertyMetadata(string.Empty));

        public bool HasReturn
        {
            get { return (bool)GetValue(HasReturnProperty); }
            set { SetValue(HasReturnProperty, value); }
        }

        public string ReturnText
        {
            get { return (string)GetValue(ReturnTextProperty); }
            set { SetValue(ReturnTextProperty, value); }
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
    }
}
