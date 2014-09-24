using Magellan.Framework;
using TaxCalculator.Features.Tax.Model;

namespace TaxCalculator.Features.Tax.Views.Submit
{
    public class SubmitViewModel : ViewModel
    {
        private readonly TaxEstimate _estimate;

        public SubmitViewModel(TaxEstimate estimate)
        {
            _estimate = estimate;
        }

        public TaxEstimate Estimate
        {
            get { return _estimate; }
        }
    }
}
