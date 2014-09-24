using FluentValidation;

namespace TaxCalculator.Features.Tax.Views.EnterDetails
{
    public class EnterDetailsViewModelValidator : AbstractValidator<EnterDetailsViewModel>
    {
        public EnterDetailsViewModelValidator()
        {
            RuleFor(x => x.GrossIncome).GreaterThan(0).WithMessage("Please enter a positive gross income for your tax return");
        }
    }
}
