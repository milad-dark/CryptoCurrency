using Cryptocurrency.Domain.Models;

namespace Cryptocurrency.Domain.Interfaces;

public interface ICryptoRepository
{
    Task<CryptoSymbol> GetCryptoSymbolAsync(string symbol);
    Task SaveCryptoSymbolAsync(CryptoSymbol cryptoSymbol);
    Task SaveSearchHistoryAsync(int userId, int cryptoSymbolId);
    Task<IEnumerable<SearchHistory>> GetSearchHistoryByUserAsync(int userId);
}
