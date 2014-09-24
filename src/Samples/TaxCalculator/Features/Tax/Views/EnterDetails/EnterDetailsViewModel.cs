using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using Magellan;
using Magellan.Framework;
using TaxCalculator.Features.Tax.Model;

namespace TaxCalculator.Features.Tax.Views.EnterDetails
{
    public class EnterDetailsViewModel : ViewModel
    {
        public EnterDetailsViewModel()
        {
            Submit = new RelayCommand(SubmitExecuted);
        }

        public ICommand Submit { get; private set; }

        [Display(Name="Gross income")]
        public decimal GrossIncome { get; set; }
        
        public TaxPeriod Period { get; set; }

        private void SubmitExecuted()
        {
            Navigator.Navigate<TaxController>(x => x.Submit(this));
        }
    }
}
