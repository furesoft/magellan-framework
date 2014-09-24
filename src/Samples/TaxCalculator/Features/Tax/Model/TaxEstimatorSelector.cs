using System.Collections.Generic;

namespace TaxCalculator.Features.Tax.Model
{
    public class TaxEstimatorSelector : ITaxEstimatorSelector
    {
        private readonly Dictionary<TaxPeriod, ITaxEstimator> _estimatorsByPeriod = new Dictionary<TaxPeriod, ITaxEstimator>();

        public void AddTaxRate(TaxPeriod period, ITaxEstimator estimator)
        {
            _estimatorsByPeriod.Add(period, estimator);
        }

        public ITaxEstimator Select(TaxPeriod period)
        {
            return _estimatorsByPeriod[period];
        }
    }
}