using CurrencyExchange.API.Controllers;
using CurrencyExchange.API.Models.Request;
using CurrencyExchange.Core.Entities;
using CurrencyExchange.Core.Enums;
using CurrencyExchange.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CurrencyExchange.Tests.Unit.Controllers
{
    public class CurrencyExchangeControllerTests
    {
        [Fact]
        public async Task Post_ValidRequest_ReturnsCreatedResult()
        {
            // Arrange
            var exchangeServiceMock = new Mock<IExchangeService>();
            var clientServiceMock = new Mock<IClientService>();

            clientServiceMock.Setup(s => s.IsClientRateLimitExceededAsync(It.IsAny<int>())).ReturnsAsync(false);
            exchangeServiceMock.Setup(s => s.ExchangeAsync(It.IsAny<int>(), It.IsAny<decimal>(), It.IsAny<CurrencyType>(), It.IsAny<CurrencyType>()))
                .ReturnsAsync(new CurrencyExchangeTransaction
                {
                    Id = 1,
                    ClientId = 1,
                    Amount = 100,
                    Rate = 1.2m,
                    SourceCurrency = CurrencyType.USD,
                    TargetCurrency = CurrencyType.EUR
                });

            var controller = new CurrencyExchangeController(exchangeServiceMock.Object, clientServiceMock.Object);

            // Act
            var result = await controller.Post(new CurrencyExchangeRequest
            {
                ClientId = 1,
                Amount = 100,
                SourceCurrency = CurrencyType.USD,
                TargetCurrency = CurrencyType.EUR
            });

            // Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async Task Post_RateLimitExceeded_ReturnsTooManyRequestsResult()
        {
            // Arrange
            var exchangeServiceMock = new Mock<IExchangeService>();
            var clientServiceMock = new Mock<IClientService>();

            clientServiceMock.Setup(s => s.IsClientRateLimitExceededAsync(It.IsAny<int>())).ReturnsAsync(true);

            var controller = new CurrencyExchangeController(exchangeServiceMock.Object, clientServiceMock.Object);

            // Act
            var result = await controller.Post(new CurrencyExchangeRequest
            {
                ClientId = 1,
                Amount = 100,
                SourceCurrency = CurrencyType.USD,
                TargetCurrency = CurrencyType.EUR
            });

            // Assert
            Assert.IsType<StatusCodeResult>(result.Result);
            Assert.Equal(429, ((StatusCodeResult)result.Result).StatusCode);
        }
    }
}
