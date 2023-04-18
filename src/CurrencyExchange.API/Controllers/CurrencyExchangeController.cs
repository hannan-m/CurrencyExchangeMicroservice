using CurrencyExchange.API.Models.Request;
using CurrencyExchange.API.Models.Response;
using CurrencyExchange.Core.Entities;
using CurrencyExchange.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyExchangeController : ControllerBase
    {
        private readonly IExchangeService _exchangeService;
        private readonly IClientService _clientService;

        public CurrencyExchangeController(IExchangeService exchangeService, IClientService clientService)
        {
            _exchangeService = exchangeService;
            _clientService = clientService;
        }

        // POST api/currencyexchange
        [HttpPost]
        public async Task<ActionResult<CurrencyExchangeResponse>> Post(CurrencyExchangeRequest request)
        {
            // client verification is not done
            // test project focses on the task more than adding other features

            if (await _clientService.IsClientRateLimitExceededAsync(request.ClientId))
            {
                return StatusCode(429); // Too Many Requests
            }

            var transaction = await _exchangeService.ExchangeAsync(request.ClientId, request.Amount, request.SourceCurrency, request.TargetCurrency);

            var response = new CurrencyExchangeResponse
            {
                TransactionId = transaction.Id,
                Amount = transaction.Amount,
                ConvertedAmount = transaction.Amount * transaction.Rate,
                SourceCurrency = transaction.SourceCurrency,
                TargetCurrency = transaction.TargetCurrency
            };

            return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, response);
        }

        // GET api/currencyexchange/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CurrencyExchangeTransaction>> GetTransaction(int id)
        {
            var transaction = await _exchangeService.GetTransactionAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return transaction;
        }
    }
}
