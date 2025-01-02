using Cryptocurrency.Application.Cache;
using Cryptocurrency.Application.Dtos;
using Cryptocurrency.Application.Logger;
using Cryptocurrency.Domain.Interfaces;
using Cryptocurrency.Domain.Models;
using Cryptocurrency.Infrastructure.ServiceProxy;

namespace Cryptocurrency.Application.Crypto;

public class CryptoApplication : ICryptoApplication
{
    private readonly ILoggerService<CryptoApplication> _logger;
    private readonly ICoinMarketCapApi _coinMarketCapApi;
    private readonly IExchangeRatesApi _exchangeRatesApi;
    private readonly ICryptoRepository _cryptoRepository;
    private readonly ICacheService _cacheService;
    private const string ExchangeRatesApiKey = "f98dab572235feed5c651a18c2c13228";
    private static readonly string[] SupportedCurrencies = ["USD", "EUR", "BRL", "GBP", "AUD"];


    public CryptoApplication(
        ICoinMarketCapApi coinMarketCapApi,
        IExchangeRatesApi exchangeRatesApi,
        ICryptoRepository cryptoRepository,
        ILoggerService<CryptoApplication> logger,
        ICacheService cacheService)
    {
        _coinMarketCapApi = coinMarketCapApi;
        _exchangeRatesApi = exchangeRatesApi;
        _cryptoRepository = cryptoRepository;
        _logger = logger;
        _cacheService = cacheService;
    }

    public async Task<Dictionary<string, decimal>> GetCryptoSymbolRatesAsync(string cryptoCode, int userId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(cryptoCode))
            {
                _logger.LogError("Invalid cryptoCode: cryptoCode is null or empty.");
                throw new ArgumentException("cryptoCode cannot be null or empty.", nameof(cryptoCode));
            }

            cryptoCode = cryptoCode.ToUpper();

            var cacheKey = $"CryptoRates:{cryptoCode}";
            var cachedRates = await _cacheService.GetAsync<Dictionary<string, decimal>>(cacheKey);
            if (cachedRates != null)
            {
                _logger.LogInformation($"Returning cached data for {cryptoCode}");
                return cachedRates;
            }

            var coinMarketCapApi = await _coinMarketCapApi.GetCryptoPriceAsync(cryptoCode);
            var exchangeRatesApi = await _exchangeRatesApi.GetExchangeRatesAsync(ExchangeRatesApiKey);

            if (exchangeRatesApi?.Rates == null)
            {
                _logger.LogError("Exchange rates API returned no data.");
                throw new InvalidOperationException("Failed to fetch exchange rates.");
            }

            var price = coinMarketCapApi.Data[cryptoCode].Quote["USD"].Price;
            var exchangeRates = SupportedCurrencies
                .AsParallel()
                .Where(currency => exchangeRatesApi.Rates.ContainsKey(currency))
                .ToDictionary(
                    currency => currency,
                    currency => price * exchangeRatesApi.Rates[currency]
                );

            await _cacheService.SetAsync(cacheKey, exchangeRates, TimeSpan.FromMinutes(10));

            _ = Task.Run(() => SaveCryptoSymbolAndHistory(cryptoCode, userId, exchangeRates));

            return exchangeRates;
        }
        catch (ArgumentException ex)
        {
            _logger.LogError("Validation error in GetCryptoSymbolRatesAsync.", ex);
            throw new ArgumentException("Validation error in GetCryptoSymbolRatesAsync.", ex);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError("Data missing in GetCryptoSymbolRatesAsync.", ex);
            throw new KeyNotFoundException("Data missing in GetCryptoSymbolRatesAsync.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError("An unexpected error occurred in GetCryptoSymbolRatesAsync.", ex);
            throw new Exception("An unexpected error occurred in GetCryptoSymbolRatesAsync.", ex);
        }
    }

    public async Task<IEnumerable<SearchHistoryDto>> GetSearchHistory(int userId)
    {
        try
        {
            if (userId <= 0)
            {
                _logger.LogError("Invalid userId: must be greater than zero.");
                throw new ArgumentException("UserId must be greater than zero.", nameof(userId));
            }

            var cacheKey = $"SearchHistory_{userId}";
            var searchHistoriesCashe = await _cacheService.GetAsync<IEnumerable<SearchHistoryDto>>(cacheKey);
            if (searchHistoriesCashe != null)
            {
                _logger.LogInformation($"Returning cached data for history userId: {userId}");
                return searchHistoriesCashe;
            }

            var searchHistories = await _cryptoRepository.GetSearchHistoryByUserAsync(userId);
            _logger.LogInformation($"Retrieved search history for userId: {userId}");

            var result = searchHistories.Select(sh => new SearchHistoryDto
            {
                Id = sh.Id,
                UserId = sh.UserId,
                SearchedAt = sh.SearchedAt,
                CryptoSymbol = new CryptoSymbolDto
                {
                    Symbol = sh.CryptoSymbol.Symbol,
                    ExchangeRates = sh.CryptoSymbol.ExchangeRates?.Select(er => new ExchangeRatesDto
                    {
                        Currency = er.Currency,
                        Price = er.Price,
                        Timestamp = er.Timestamp
                    }).ToList()
                }
            });

            await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(10));
            return result;
        }
        catch (ArgumentException ex)
        {
            _logger.LogError("Validation error in GetSearchHistory.", ex);
            throw new ArgumentException("Validation error in GetSearchHistory.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError("get history user has error", ex);
            return Enumerable.Empty<SearchHistoryDto>();
        }
    }

    private async Task SaveCryptoSymbolAndHistory(string cryptoCode, int userId, Dictionary<string, decimal> exchangeRates)
    {
        _logger.LogInformation("Start saving Crypto symbol and history for user");
        var timeStamp = DateTime.UtcNow;
        var exchangeRateList = exchangeRates.Select(exr => new ExchangeRates
        {
            Currency = exr.Key,
            Price = exr.Value,
            Timestamp = timeStamp
        }).ToList();

        var newSymbol = new CryptoSymbol
        {
            Symbol = cryptoCode,
            UserId = userId,
            ExchangeRates = exchangeRateList
        };

        try
        {
            await _cryptoRepository.SaveCryptoSymbolAsync(newSymbol);
            await _cryptoRepository.SaveSearchHistoryAsync(userId, newSymbol.Id);
            _logger.LogInformation("End saving Crypto symbol and history for user");
        }
        catch (Exception ex)
        {
            _logger.LogError("Saving crypto symbol or history has error", ex);
        }
    }

}
