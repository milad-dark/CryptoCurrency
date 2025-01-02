using Cryptocurrency.Domain.Interfaces;
using Cryptocurrency.Domain.Models;
using Cryptocurrency.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Cryptocurrency.Infrastructure.Repositories;

public class CryptoRepository : ICryptoRepository
{
    private readonly IDbContextFactory<ApplicationContext> _contextFactory;

    public CryptoRepository(IDbContextFactory<ApplicationContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<CryptoSymbol> GetCryptoSymbolAsync(string symbol)
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.CryptoSymbols.FirstOrDefaultAsync(c => c.Symbol == symbol);
    }

    public async Task SaveCryptoSymbolAsync(CryptoSymbol cryptoSymbol)
    {
        using var context = _contextFactory.CreateDbContext();
        context.CryptoSymbols.Add(cryptoSymbol);
        await context.SaveChangesAsync();
    }

    public async Task SaveSearchHistoryAsync(int userId, int cryptoSymbolId)
    {
        using var context = _contextFactory.CreateDbContext();
        var history = new SearchHistory
        {
            UserId = userId,
            CryptoSymbolId = cryptoSymbolId,
            SearchedAt = DateTime.UtcNow
        };

        context.SearchHistories.Add(history);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<SearchHistory>> GetSearchHistoryByUserAsync(int userId)
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.SearchHistories
            .Include(sh => sh.CryptoSymbol)
            .ThenInclude(ex => ex.ExchangeRates)
            .Where(sh => sh.UserId == userId)
            .ToListAsync();
    }
}
