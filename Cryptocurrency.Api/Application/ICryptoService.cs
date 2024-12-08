namespace Cryptocurrency.Api.Application
{
    public interface ICryptoService
    {
        Task<Dictionary<string, decimal>> GetCryptoRatesAsync(string cryptoCode);
    }
}
