using CurrencyExchange.Core.Entities;

namespace CurrencyExchange.Core.Interfaces
{
    public interface ITransactionRepository : IRepository<CurrencyExchangeTransaction>
    {
        Task<int> GetTransactionCountInLastHour(int clientId);
    }
}
