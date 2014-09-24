using System.Windows.Input;
using Magellan;
using Magellan.Framework;
using TaxCalculator.Features.Tax;

namespace TaxCalculator.Features.Home.Views.Index
{
    public class IndexViewModel : ViewModel
    {
        public IndexViewModel()
        {
            About = new RelayCommand(AboutExecuted);
            Start = new RelayCommand(StartExecuted);
        }

        public ICommand About { get; private set; }
        public ICommand Start { get; private set; }

        private void AboutExecuted()
        {
            Navigator.Navigate<HomeController>(x => x.About());
        }

        private void StartExecuted()
        {
            Navigator.Navigate<TaxController>(x => x.EnterDetails());
        }
    }
}
