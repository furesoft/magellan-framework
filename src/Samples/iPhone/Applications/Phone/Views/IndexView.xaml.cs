using System.Windows.Controls;
using Magellan;

namespace iPhone.Applications.Phone.Views
{
    public partial class IndexView : Page, INavigationAware
    {
        public IndexView()
        {
            InitializeComponent();
            var hasLoaded = false;
            Loaded += (x, y) =>
            {
                if (hasLoaded) 
                    return;
                hasLoaded = true;
                
                Navigator.Factory.CreateNavigator(ContactsFrame).Navigate(new { controller = "Phone", action = "Groups" });
                Navigator.Factory.CreateNavigator(KeypadFrame).Navigate(new {controller = "Phone", action = "Keypad"});
            };
        }

        public INavigator Navigator { get; set; }
    }
}
