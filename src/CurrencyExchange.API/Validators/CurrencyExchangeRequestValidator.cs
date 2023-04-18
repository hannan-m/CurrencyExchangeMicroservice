using CurrencyExchange.API.Models.Request;
using FluentValidation;

namespace CurrencyExchange.API.Validators
{
    public class CurrencyExchangeRequestValidator : AbstractValidator<CurrencyExchangeRequest> 
    {
        public CurrencyExchangeRequestValidator()
        {
            RuleFor(x=> x.ClientId).NotEmpty();

            RuleFor(x => x.Amount).NotEmpty().GreaterThan(0);
            RuleFor(x => x.SourceCurrency).NotEmpty().IsInEnum();
            RuleFor(x => x.TargetCurrency).NotEmpty().IsInEnum();
        }
    }
}
