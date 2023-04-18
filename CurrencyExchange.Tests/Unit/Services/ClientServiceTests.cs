using CurrencyExchange.Core.Entities;
using CurrencyExchange.Core.Enums;
using CurrencyExchange.Core.Interfaces;
using CurrencyExchange.Core.Services;
using Moq;
using Xunit;

namespace CurrencyExchange.Tests.Unit.Services
{
    public class ClientServiceTests
    {
        [Fact]
        public async Task IsClientRateLimitExceededAsync_RateLimitNotExceeded_ReturnsFalse()
        {
            // Arrange
            var transactionRepositoryMock = new Mock<ITransactionRepository>();

            var transactions = Enumerable.Range(1, 5).Select(i => new CurrencyExchangeTransaction
            {
                Id = i,
                ClientId = 1,
                Timestamp = DateTime.UtcNow.AddMinutes(-10 * i),
                Amount = 100,
                Rate = 1.2m,
                SourceCurrency = CurrencyType.USD,
                TargetCurrency = CurrencyType.EUR
            }).ToList();

            transactionRepositoryMock.Setup(r => r.GetAll()).Returns(Task.FromResult(transactions));

            var clientService = new ClientService(transactionRepositoryMock.Object);

            // Act
            bool isRateLimitExceeded = await clientService.IsClientRateLimitExceededAsync(1);

            // Assert
            Assert.False(isRateLimitExceeded);
        }

        [Fact]
        public async Task IsClientRateLimitExceededAsync_RateLimitExceeded_ReturnsTrue()
        {
            // Arrange
            var transactionRepositoryMock = new Mock<ITransactionRepository>();

            var transactions = Enumerable.Range(1, 15).Select(i => new CurrencyExchangeTransaction
            {
                Id = i,
                ClientId = 1,
                Timestamp = DateTime.UtcNow.AddMinutes(-10 * i),
                Amount = 100,
                Rate = 1.2m,
                SourceCurrency = CurrencyType.USD,
                TargetCurrency = CurrencyType.EUR
            }).ToList();

            transactionRepositoryMock.Setup(r => r.GetAll()).Returns(Task.FromResult(transactions));
            transactionRepositoryMock.Setup(r => r.GetTransactionCountInLastHour(1)).Returns(Task.FromResult(12));
            var clientService = new ClientService(transactionRepositoryMock.Object);

            // Act
            bool isRateLimitExceeded = await clientService.IsClientRateLimitExceededAsync(1);

            // Assert
            Assert.True(isRateLimitExceeded);
        }
    }
}
