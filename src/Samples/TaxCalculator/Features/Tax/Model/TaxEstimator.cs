namespace TaxCalculator.Features.Tax.Model
{
    public class TaxEstimator : ITaxEstimator
    {
        private readonly TaxBracketSelector _bracketSelector;
        private readonly ITaxModifier[] _modifiers;

        public TaxEstimator(TaxBracketSelector bracketSelector, params ITaxModifier[] modifiers)
        {
            _bracketSelector = bracketSelector;
            _modifiers = modifiers;
        }

        public TaxEstimate Estimate(Situation situation)
        {
            var estimate = new TaxEstimate(situation);
            var bracket = _bracketSelector.SelectBracket(situation);
            estimate.TaxOnIncome = bracket.CalculateTaxPayable(situation);

            foreach (var modifier in _modifiers)
            {
                modifier.Apply(estimate);
            }

            return estimate;
        }
    }
}