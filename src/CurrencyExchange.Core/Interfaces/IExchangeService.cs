using CurrencyExchange.Core.Entities;

namespace CurrencyExchange.Core.Interfaces
{
    public interface IExchangeService
    {
        Task<CurrencyExchangeTransaction> ExchangeAsync(int clientId, decimal amount, Enums.CurrencyType sourceCurrency, Enums.CurrencyType targetCurrency);
        Task<CurrencyExchangeTransaction> GetTransactionAsync(int id);
    }
}
