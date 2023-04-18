using CurrencyExchange.Core.Enums;

namespace CurrencyExchange.API.Models.Request
{
    public class CurrencyExchangeRequest
    {
        public int ClientId { get; set; }
        public decimal Amount { get; set; }
        public CurrencyType SourceCurrency { get; set; }
        public CurrencyType TargetCurrency { get; set; }
    }
}
