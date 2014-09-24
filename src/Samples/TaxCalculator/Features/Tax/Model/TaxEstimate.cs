namespace TaxCalculator.Features.Tax.Model
{
    public class TaxEstimate
    {
        private readonly Situation _situation;

        public TaxEstimate(Situation situation)
        {
            _situation = situation;
        }

        public Situation Situation
        {
            get { return _situation; }
        }

        public decimal TaxOnIncome { get; set; }
        
        public decimal MedicareLevy { get; set; }
        
        public decimal NetIncome
        {
            get { return Situation.GrossIncome - (TaxOnIncome + MedicareLevy); }
        }
    }
}