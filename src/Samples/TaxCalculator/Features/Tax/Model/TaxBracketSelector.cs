using System.Linq;

namespace TaxCalculator.Features.Tax.Model
{
    public class TaxBracketSelector
    {
        private readonly TaxBracket[] _brackets;

        public TaxBracketSelector(params TaxBracket[] brackets)
        {
            _brackets = brackets;
        }

        public TaxBracket SelectBracket(Situation situation)
        {
            return _brackets.Single(x => x.IsWithinBracket(situation));
        }
    }
}