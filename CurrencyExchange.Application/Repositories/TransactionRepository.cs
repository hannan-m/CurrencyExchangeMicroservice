using CurrencyExchange.Core.Entities;
using CurrencyExchange.Core.Interfaces;
using CurrencyExchange.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.Application.Repositories
{
    public class TransactionRepository : Repository<CurrencyExchangeTransaction>, ITransactionRepository
    {
        private readonly CurrencyExchangeDbContext _currencyExchangeDbContext;
        public TransactionRepository(CurrencyExchangeDbContext dbContext) : base(dbContext)
        {
            _currencyExchangeDbContext = dbContext;
        }

        public async Task<int> GetTransactionCountInLastHour(int clientId)
        {
            DateTime oneHourAgo = DateTime.UtcNow.AddHours(-1);
            return await _currencyExchangeDbContext.CurrencyExchangeTransactions.Where(t => t.ClientId == clientId && t.Timestamp >= oneHourAgo)
                .CountAsync();
        }
    }
}
