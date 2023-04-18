using CurrencyExchange.Core.Entities;
using CurrencyExchange.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CurrencyExchange.Infrastructure.DataAccess
{
    public class CurrencyExchangeDbContextInitialiser
    {
        private readonly ILogger<CurrencyExchangeDbContextInitialiser> _logger;
        private readonly CurrencyExchangeDbContext _context;

        public CurrencyExchangeDbContextInitialiser(ILogger<CurrencyExchangeDbContextInitialiser> logger, CurrencyExchangeDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task InitialiseAsync()
        {
            try
            {
                if (_context.Database.IsRelational())
                {
                    await _context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initialising the database.");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        public async Task TrySeedAsync()
        {
            // Default data
            // Seed, if necessary
            if (!_context.Clients.Any())
            {
                // Seed clients
                _context.Clients.AddRange(
                    new Client { Id = 1, Name = "Client 1", Email = "client1@medirect.com" },
                    new Client { Id = 2, Name = "Client 2", Email = "client2@medirect.com" },
                    new Client { Id = 3, Name = "Client 3", Email = "client3@medirect.com" }
                    );

                // Seed transactions
                _context.CurrencyExchangeTransactions.AddRange(
                    new CurrencyExchangeTransaction
                    {
                        Id = 1,
                        ClientId = 1,
                        Timestamp = DateTime.UtcNow.AddHours(-1),
                        Amount = 100,
                        Rate = 1.2m,
                        SourceCurrency = CurrencyType.USD,
                        TargetCurrency = CurrencyType.EUR
                    },
                    new CurrencyExchangeTransaction
                    {
                        Id = 2,
                        ClientId = 2,
                        Timestamp = DateTime.UtcNow.AddHours(-2),
                        Amount = 200,
                        Rate = 0.8m,
                        SourceCurrency = CurrencyType.EUR,
                        TargetCurrency = CurrencyType.USD
                    },
                    new CurrencyExchangeTransaction
                    {
                        Id = 3,
                        ClientId = 3,
                        Timestamp = DateTime.UtcNow.AddHours(-3),
                        Amount = 150,
                        Rate = 1.1m,
                        SourceCurrency = CurrencyType.USD,
                        TargetCurrency = CurrencyType.GBP
                    }
                );

                await _context.SaveChangesAsync();
            }
        }
    }
}
