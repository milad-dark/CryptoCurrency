using Cryptocurrency.Infrastructure.Models;
using Refit;

namespace Cryptocurrency.Infrastructure.ServiceProxy;

public interface IExchangeRatesApi
{
    [Get("/latest?access_key={access_key}")]
    Task<ExchangeRatesResponse> GetExchangeRatesAsync(string access_key);
}
