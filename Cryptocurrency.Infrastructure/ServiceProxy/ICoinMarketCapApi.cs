using Cryptocurrency.Infrastructure.Models;
using Refit;

namespace Cryptocurrency.Infrastructure.ServiceProxy;

public interface ICoinMarketCapApi
{
    [Get("/cryptocurrency/quotes/latest")]
    Task<CoinMarketCapResponse> GetCryptoPriceAsync([AliasAs("symbol")] string cryptoCode);
    [Get("/cryptocurrency/quotes/latest")]
    Task<CoinMarketCapResponse> GetCryptoPriceOnExchangeAsync([AliasAs("symbol")] string cryptoCode, [AliasAs("convert")] string convert_Exchange);
}
