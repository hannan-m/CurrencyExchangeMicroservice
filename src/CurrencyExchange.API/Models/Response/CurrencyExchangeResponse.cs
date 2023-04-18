using CurrencyExchange.Core.Enums;

namespace CurrencyExchange.API.Models.Response
{
    public class CurrencyExchangeResponse
    {
        public int TransactionId { get; set; }
        public decimal Amount { get; set; }
        public decimal ConvertedAmount { get; set; }
        public CurrencyType SourceCurrency { get; set; }
        public CurrencyType TargetCurrency { get; set; }
    }
}
