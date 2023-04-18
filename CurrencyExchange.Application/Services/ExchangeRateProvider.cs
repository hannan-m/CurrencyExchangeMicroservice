using Newtonsoft.Json.Linq;
using CurrencyExchange.Core.Enums;
using CurrencyExchange.Core.Interfaces;
using CurrencyExchange.Core.Settings;
using Microsoft.Extensions.Options;
using Azure.Core;

namespace CurrencyExchange.Core.Services
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ExchangeRateProviderSettings _settings;

        public ExchangeRateProvider(HttpClient httpClient, IOptions<ExchangeRateProviderSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
        }

        public async Task<decimal> GetExchangeRateAsync(CurrencyType fromCurrency, CurrencyType toCurrency)
        {
           
            string endpoint = $"{_settings.BaseUrl}/exchangerates_data/latest?symbols={toCurrency}&base={fromCurrency}";
            _httpClient.DefaultRequestHeaders.Add("apikey", _settings.ApiKey);

            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error fetching exchange rate: {response.ReasonPhrase}");
            }

            string content = await response.Content.ReadAsStringAsync();
            JObject? rates = JObject.Parse(content)["rates"] as JObject;
            return rates[toCurrency.ToString()].Value<decimal>();
        }
    }
}
