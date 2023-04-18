using CurrencyExchange.Core.Entities;
using CurrencyExchange.Core.Enums;
using CurrencyExchange.Core.Interfaces;
using CurrencyExchange.Core.Services;
using CurrencyExchange.Infrastructure.Caching;
using Moq;
using Xunit;

namespace CurrencyExchange.Tests.Unit.Services
{
    public class ExchangeServiceTests
    {
        [Fact]
        public async Task ExchangeAsync_ValidRequest_ReturnsTransaction()
        {
            // Arrange
            var transactionRepositoryMock = new Mock<IRepository<CurrencyExchangeTransaction>>();
            var clientRepositoryMock = new Mock<IRepository<Client>>();
            var exchangeRateProviderMock = new Mock<IExchangeRateProvider>();
            var exchangeRateCacheMock = new Mock<IExchangeRateCache>();

            clientRepositoryMock.Setup(r => r.ExistsAsync(It.IsAny<int>())).ReturnsAsync(true);
            exchangeRateCacheMock.Setup(c => c.GetExchangeRate(It.IsAny<CurrencyType>(), It.IsAny<CurrencyType>())).Returns((decimal?)null);
            exchangeRateProviderMock.Setup(p => p.GetExchangeRateAsync(It.IsAny<CurrencyType>(), It.IsAny<CurrencyType>())).ReturnsAsync(1.2m);

            var exchangeService = new ExchangeService(transactionRepositoryMock.Object, clientRepositoryMock.Object, exchangeRateProviderMock.Object, exchangeRateCacheMock.Object);

            // Act
            var transaction = await exchangeService.ExchangeAsync(1, 100, CurrencyType.USD, CurrencyType.EUR);

            // Assert
            Assert.NotNull(transaction);
            Assert.Equal(1, transaction.ClientId);
            Assert.Equal(100, transaction.Amount);
            Assert.Equal(1.2m, transaction.Rate);
            Assert.Equal(CurrencyType.USD, transaction.SourceCurrency);
            Assert.Equal(CurrencyType.EUR, transaction.TargetCurrency);

            transactionRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<CurrencyExchangeTransaction>()), Times.Once);
        }
    }
}
