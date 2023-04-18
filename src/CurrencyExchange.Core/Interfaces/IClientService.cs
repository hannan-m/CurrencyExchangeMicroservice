namespace CurrencyExchange.Core.Interfaces
{
    public interface IClientService
    {
        Task<bool> IsClientRateLimitExceededAsync(int clientId);
    }
}
