namespace TaxCalculator.Features.Tax.Model
{
    public interface ITaxEstimator
    {
        TaxEstimate Estimate(Situation situation);
    }
}