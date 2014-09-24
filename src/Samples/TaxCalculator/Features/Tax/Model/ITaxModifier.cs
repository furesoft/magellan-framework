namespace TaxCalculator.Features.Tax.Model
{
    public interface ITaxModifier
    {
        void Apply(TaxEstimate estimate);
    }
}