namespace CurrencyExchange.Core.Entities
{
    public class CurrencyExchangeTransaction
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal Amount { get; set; }
        public decimal Rate { get; set; }
        public Enums.CurrencyType SourceCurrency { get; set; }
        public Enums.CurrencyType TargetCurrency { get; set; }
    }
}
