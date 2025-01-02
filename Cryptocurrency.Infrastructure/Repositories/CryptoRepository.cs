using Cryptocurrency.Domain.Interfaces;
using Cryptocurrency.Domain.Models;
using Cryptocurrency.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Cryptocurrency.Infrastructure.Repositories;

public class CryptoRepository : ICryptoRepository
{
    private readonly ApplicationContext _context;

    public CryptoRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<CryptoSymbol> GetCryptoSymbolAsync(string symbol)
    {
        return await _context.CryptoSymbols.FirstOrDefaultAsync(c => c.Symbol == symbol);
    }

    public async Task SaveCryptoSymbolAsync(CryptoSymbol cryptoSymbol)
    {
        _context.CryptoSymbols.Add(cryptoSymbol);
        await _context.SaveChangesAsync();
    }

    public async Task SaveSearchHistoryAsync(int userId, int cryptoSymbolId)
    {
        var history = new SearchHistory
        {
            UserId = userId,
            CryptoSymbolId = cryptoSymbolId,
            SearchedAt = DateTime.UtcNow
        };

        _context.SearchHistories.Add(history);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<SearchHistory>> GetSearchHistoryByUserAsync(int userId)
    {
        return await _context.SearchHistories
            .Include(sh => sh.CryptoSymbol)
            .ThenInclude(ex => ex.ExchangeRates)
            .Where(sh => sh.UserId == userId)
            .ToListAsync();
    }
}
