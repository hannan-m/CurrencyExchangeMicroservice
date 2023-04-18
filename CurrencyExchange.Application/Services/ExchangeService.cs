using CurrencyExchange.Core.Entities;
using CurrencyExchange.Core.Enums;
using CurrencyExchange.Core.Interfaces;
using CurrencyExchange.Infrastructure.Caching;

namespace CurrencyExchange.Core.Services
{
    public class ExchangeService : IExchangeService
    {
        private readonly IRepository<CurrencyExchangeTransaction> _transactionRepository;
        private readonly IRepository<Client> _clientRepository;
        private readonly IExchangeRateProvider _exchangeRateProvider;
        private readonly IExchangeRateCache _exchangeRateCache;
        private readonly TimeSpan _exchangeRateCacheDuration = TimeSpan.FromMinutes(30);

        public ExchangeService(IRepository<CurrencyExchangeTransaction> transactionRepository,
            IRepository<Client> clientRepository, IExchangeRateProvider exchangeRateProvider,
            IExchangeRateCache exchangeRateCache)
        {
            _transactionRepository = transactionRepository;
            _clientRepository = clientRepository;
            _exchangeRateProvider = exchangeRateProvider;
            _exchangeRateCache = exchangeRateCache;
        }

        public async Task<CurrencyExchangeTransaction> ExchangeAsync(int clientId, decimal amount, CurrencyType sourceCurrency, CurrencyType targetCurrency)
        {
            // Check if client exists
            if (!await _clientRepository.ExistsAsync(clientId))
            {
                throw new ArgumentException("Client does not exist");
            }

            decimal rate = _exchangeRateCache.GetExchangeRate(sourceCurrency, targetCurrency) ??
                           await FetchAndCacheExchangeRate(sourceCurrency, targetCurrency);

            CurrencyExchangeTransaction transaction = new CurrencyExchangeTransaction
            {
                ClientId = clientId,
                Timestamp = DateTime.UtcNow,
                Amount = amount,Rate = rate,
                SourceCurrency = sourceCurrency,
                TargetCurrency = targetCurrency
            };

            await _transactionRepository.CreateAsync(transaction);
            return transaction;
        }

        private async Task<decimal> FetchAndCacheExchangeRate(CurrencyType sourceCurrency, CurrencyType targetCurrency)
        {
            decimal rate = await _exchangeRateProvider.GetExchangeRateAsync(sourceCurrency, targetCurrency);
            _exchangeRateCache.SetExchangeRate(sourceCurrency, targetCurrency, rate, _exchangeRateCacheDuration);
            return rate;
        }

        public async Task<CurrencyExchangeTransaction> GetTransactionAsync(int id)
        {
            var transaction = await _transactionRepository.GetAsync(id);
            return transaction;
        }
    }
}
