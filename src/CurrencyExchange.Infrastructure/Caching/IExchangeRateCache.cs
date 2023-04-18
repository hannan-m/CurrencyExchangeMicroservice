using CurrencyExchange.Core.Enums;

namespace CurrencyExchange.Infrastructure.Caching
{
    public interface IExchangeRateCache
    {
        void SetExchangeRate(CurrencyType fromCurrency, CurrencyType toCurrency, decimal rate, TimeSpan duration);
        decimal? GetExchangeRate(CurrencyType fromCurrency, CurrencyType toCurrency);
    }
}
