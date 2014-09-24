namespace TaxCalculator.Features.Tax.Model
{
    public interface ITaxEstimatorSelector
    {
        ITaxEstimator Select(TaxPeriod period);
    }
}