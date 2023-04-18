using CurrencyExchange.Core.Enums;

namespace CurrencyExchange.Core.Interfaces
{
    public interface IExchangeRateProvider
    {
        Task<decimal> GetExchangeRateAsync(CurrencyType fromCurrency, CurrencyType toCurrency);
    }
}
