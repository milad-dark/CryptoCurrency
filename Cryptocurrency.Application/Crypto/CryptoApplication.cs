using Cryptocurrency.Application.Dtos;
using Cryptocurrency.Application.Logger;
using Cryptocurrency.Domain.Interfaces;
using Cryptocurrency.Domain.Models;
using Cryptocurrency.Infrastructure.ServiceProxy;

namespace Cryptocurrency.Application.Crypto
{
    public class CryptoApplication : ICryptoApplication
    {
        private readonly ILoggerService _logger;
        private readonly ICoinMarketCapApi _coinMarketCapApi;
        private readonly IExchangeRatesApi _exchangeRatesApi;
        private readonly ICryptoRepository _cryptoRepository;
        private const string ExchangeRatesApiKey = "f98dab572235feed5c651a18c2c13228";

        public CryptoApplication(ICoinMarketCapApi coinMarketCapApi, IExchangeRatesApi exchangeRatesApi, ICryptoRepository cryptoRepository, ILoggerService logger)
        {
            _coinMarketCapApi = coinMarketCapApi;
            _exchangeRatesApi = exchangeRatesApi;
            _cryptoRepository = cryptoRepository;
            _logger = logger ?? throw new ArgumentNullException(nameof(LoggerService));
        }

        public async Task<Dictionary<string, decimal>> GetCryptoSymbolRatesAsync(string cryptoCode, int userId)
        {
            if (string.IsNullOrEmpty(cryptoCode))
            {
                _logger.LogError("cryptoCode is null or empty");
                throw new Exception("cryptoCode is null or empty");
            }

            cryptoCode = cryptoCode.ToUpper();

            var coinMarketCapApi = await _coinMarketCapApi.GetCryptoPriceAsync(cryptoCode);
            var exchangeRatesApi = await _exchangeRatesApi.GetExchangeRatesAsync(ExchangeRatesApiKey);

            var exchangeRates = new Dictionary<string, decimal>();
            var price = coinMarketCapApi?.Data?[cryptoCode]?.Quote["USD"].Price ?? 0;

            foreach (var currency in new[] { "USD", "EUR", "BRL", "GBP", "AUD" })
            {
                if (exchangeRatesApi.Rates.TryGetValue(currency, out var rate))
                {
                    exchangeRates[currency] = price * rate;
                }
            }

            await SaveCryptoSymbolAndHistory(cryptoCode, userId, exchangeRates);

            return exchangeRates;
        }

        public async Task<IEnumerable<SearchHistoryDto>> GetSearchHistory(int userId)
        {
            try
            {
                var searchHistories = await _cryptoRepository.GetSearchHistoryByUserAsync(userId);
                _logger.LogInformation("Get history user");

                var searchHistoryDtos = searchHistories.Select(sh => new SearchHistoryDto
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
                }).ToList();
                return searchHistoryDtos ?? Enumerable.Empty<SearchHistoryDto>();
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
}
