using CurrencyExchange.Core.Interfaces;

namespace CurrencyExchange.Core.Services
{
    public class ClientService : IClientService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly int _maxTransactionsPerHour = 10;

        public ClientService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<bool> IsClientRateLimitExceededAsync(int clientId)
        {
            
            int transactionCount = await _transactionRepository.GetTransactionCountInLastHour(clientId);

            return transactionCount >= _maxTransactionsPerHour;
        }
    }
}
