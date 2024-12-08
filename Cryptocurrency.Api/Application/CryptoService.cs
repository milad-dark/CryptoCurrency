using Cryptocurrency.Api.Domain.Models;
using Serilog;
using System.Text.Json;

namespace Cryptocurrency.Api.Application
{
    public class CryptoService : ICryptoService
    {
        private readonly HttpClient _httpClient;
        //private readonly IConnectionMultiplexer _redis;
        private const string CoinMarketCapApiKey = "68b8433a-50a6-4ffb-82ba-2ab931dfe606";
        private const string ExchangeRatesApiKey = "f98dab572235feed5c651a18c2c13228";
        private const string ExchangeRatesApiUrl = "http://api.exchangeratesapi.io/v1";
        private const string CoinMarketCapUrl = "https://pro-api.coinmarketcap.com/v1/cryptocurrency/quotes/latest";



        private readonly CryptoCacheService _cryptoCacheService;

        public CryptoService(CryptoCacheService cryptoCacheService
            /*, IConnectionMultiplexer redis*/)
        {
            _cryptoCacheService = cryptoCacheService;
            //_redis = redis;
        }


        public async Task<Dictionary<string, decimal>> GetCryptoRatesAsync(string cryptoCode)
        {
            cryptoCode = cryptoCode.ToUpper();
            _cryptoCacheService.Set(cryptoCode);
            //var db = _redis.GetDatabase();
            //string cacheKey = $"crypto:{cryptoCode}";
            //var cachedData = await db.StringGetAsync(cacheKey);

            //if (!string.IsNullOrEmpty(cachedData))
            //{
            //    Log.Information("Cache hit for cryptocurrency code: {Code}", cryptoCode);
            //    return JsonSerializer.Deserialize<Dictionary<string, decimal>>(cachedData) ?? [];
            //}

            Log.Information("Cache miss for cryptocurrency code: {Code}", cryptoCode);

            // Get crypto price in USD
            //var cryptoPrice = await GetCryptoPriceInUsdAsync(cryptoCode.ToUpper());
            //// Get exchange rates
            //var exchangeRates = await GetExchangeRatesAsync();

            // Get crypto price in USD
            //var cryptoPrice = await _coinMarketCapApi.GetCryptoPriceAsync(cryptoCode);

            // Get exchange rates
            //var exchangeRates = await _exchangeRatesApi.GetExchangeRatesAsync(ExchangeRatesApiKey);


            // Calculate values in other currencies
            var result = new Dictionary<string, decimal>();
            var result2 = new Dictionary<string, decimal>();

            //var price = cryptoPrice?.Data?[cryptoCode]?.Quote["USD"].Price ?? 0;
            //foreach (var currency in new[] { "USD", "EUR", "BRL", "GBP", "AUD" })
            //{
            //    if (exchangeRates.Rates.TryGetValue(currency, out var rate))
            //    {
            //        result[currency] = price * rate;
            //    }
            //    var res = await _coinMarketCapApi.GetCryptoPriceOnExchangeAsync(cryptoCode, currency);
            //    result2[currency] = res?.Data?[cryptoCode]?.Quote[currency].Price ?? 0;
            //}

            // Cache the result for 5 minutes
            //await db.StringSetAsync(cacheKey, JsonSerializer.Serialize(result), TimeSpan.FromMinutes(5));

            Log.Information("Cached cryptocurrency rates for: {Code}", cryptoCode);

            return result;
        }

        private async Task<decimal> GetCryptoPriceInUsdAsync(string cryptoCode)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{CoinMarketCapUrl}?symbol={cryptoCode}");
            request.Headers.Add("X-CMC_PRO_API_KEY", CoinMarketCapApiKey);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<CoinMarketCapResponse>(json);

            var status = data?.Status;
            if (status?.ErrorCode != 0)
            {
                Log.Fatal("CoinMarketCap API Status: {Timestamp}, Error Code: {ErrorCode}, Message: {ErrorMessage}", status.Timestamp, status.ErrorCode, status.ErrorMessage ?? "No errors");
            }

            Log.Debug("Fetched cryptocurrency price for {Code} from CoinMarketCap", cryptoCode);
            return data?.Data?[cryptoCode]?.Quote["USD"].Price ?? throw new Exception("Invalid crypto code");
        }

        private async Task<decimal> GetCryptoPriceOnExchange(string cryptoCode, string convert_Exchange)
        {
            var convertCurrencies = "USD,EUR,BRL,GBP,AUD";
            var request = new HttpRequestMessage(HttpMethod.Get, $"{CoinMarketCapUrl}?symbol={cryptoCode}&convert={convertCurrencies}");
            request.Headers.Add("X-CMC_PRO_API_KEY", CoinMarketCapApiKey);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<CoinMarketCapResponse>(json);

            var status = data?.Status;
            if (status?.ErrorCode != 0)
            {
                Log.Fatal("CoinMarketCap API Status: {Timestamp}, Error Code: {ErrorCode}, Message: {ErrorMessage}", status.Timestamp, status.ErrorCode, status.ErrorMessage ?? "No errors");
            }

            Log.Debug("Fetched cryptocurrency price for {Code} from CoinMarketCap", cryptoCode);
            return data?.Data?[cryptoCode]?.Quote[convert_Exchange].Price ?? throw new Exception("Invalid crypto code");
        }

        private async Task<Dictionary<string, decimal>> GetExchangeRatesAsync()
        {
            var response = await _httpClient.GetAsync($"{ExchangeRatesApiUrl}/latest?access_key={ExchangeRatesApiKey}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<ExchangeRatesResponse>(json);
            Log.Debug("Fetched exchange rates from ExchangeRatesAPI");
            return data?.Rates ?? throw new Exception("Failed to fetch exchange rates");
        }
    }
}
