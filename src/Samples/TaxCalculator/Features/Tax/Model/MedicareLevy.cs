namespace TaxCalculator.Features.Tax.Model
{
    public class MedicareLevy : ITaxModifier
    {
        private readonly decimal _levyFreeThreshold;
        private readonly decimal _levyRate;

        public MedicareLevy(decimal levyFreeThreshold, decimal levyRate)
        {
            _levyFreeThreshold = levyFreeThreshold;
            _levyRate = levyRate;
        }

        public void Apply(TaxEstimate estimate)
        {
            if (estimate.Situation.GrossIncome < _levyFreeThreshold)
                return;

            estimate.MedicareLevy = (estimate.Situation.GrossIncome)*_levyRate;
        }
    }
}