namespace TaxCalculator.Features.Tax.Model
{
    public class Situation
    {
        private readonly decimal _grossIncome;

        public Situation(decimal grossIncome)
        {
            _grossIncome = grossIncome;
        }

        public decimal GrossIncome
        {
            get { return _grossIncome; }
        }
    }
}