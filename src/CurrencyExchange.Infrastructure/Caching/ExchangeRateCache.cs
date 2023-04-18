using CurrencyExchange.Core.Enums;
using Microsoft.Extensions.Caching.Memory;

namespace CurrencyExchange.Infrastructure.Caching
{
    public class ExchangeRateCache : IExchangeRateCache
    {
        private readonly IMemoryCache _cache;

        public ExchangeRateCache(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void SetExchangeRate(CurrencyType fromCurrency, CurrencyType toCurrency, decimal rate, TimeSpan duration)
        {
            string cacheKey = $"{fromCurrency}-{toCurrency}";
            _cache.Set(cacheKey, rate, duration);
        }

        public decimal? GetExchangeRate(CurrencyType fromCurrency, CurrencyType toCurrency)
        {
            string cacheKey = $"{fromCurrency}-{toCurrency}";
            return _cache.TryGetValue(cacheKey, out decimal rate) ? rate : (decimal?)null;
        }
    }
}
