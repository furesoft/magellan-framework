
namespace TaxCalculator.Features.Tax.Model
{
    public class TaxBracket
    {
        private readonly decimal _from;
        private readonly decimal _to;
        private readonly decimal _flagfall;
        private readonly decimal _rate;

        public TaxBracket(decimal from, decimal to, decimal flagfall, decimal rate)
        {
            _from = from;
            _to = to;
            _flagfall = flagfall;
            _rate = rate;
        }

        public bool IsWithinBracket(Situation situation)
        {
            return _from < situation.GrossIncome && situation.GrossIncome <= _to;
        }

        public decimal CalculateTaxPayable(Situation situation)
        {
            return _flagfall + ((situation.GrossIncome - _from) *_rate);
        }
    }
}
