namespace CurrencyExchange.Core.Settings
{
    public class ExchangeRateProviderSettings
    {
        public string BaseUrl { get; set; }
        public string ApiKey { get; set; }
        public int CacheDurationInMinutes { get; set; }
    }
}
