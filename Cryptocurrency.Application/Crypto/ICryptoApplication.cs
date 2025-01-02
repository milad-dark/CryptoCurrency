using Cryptocurrency.Application.Dtos;

namespace Cryptocurrency.Application.Crypto;

public interface ICryptoApplication
{
    Task<Dictionary<string, decimal>> GetCryptoSymbolRatesAsync(string cryptoCode, int userId);
    Task<IEnumerable<SearchHistoryDto>> GetSearchHistory(int userId);
}
