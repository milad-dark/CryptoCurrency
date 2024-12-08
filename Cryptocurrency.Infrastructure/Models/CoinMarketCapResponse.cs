using System.Text.Json.Serialization;

namespace Cryptocurrency.Infrastructure.Models
{
    public record CoinMarketCapResponse([property: JsonPropertyName("data")] Dictionary<string, CryptoData> Data,
        [property: JsonPropertyName("status")] CoinMarketCapStatus Status);

    public class CoinMarketCapStatus
    {
        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }

        [JsonPropertyName("error_code")]
        public int ErrorCode { get; set; }

        [JsonPropertyName("error_message")]
        public string ErrorMessage { get; set; }

        [JsonPropertyName("elapsed")]
        public int Elapsed { get; set; }

        [JsonPropertyName("credit_count")]
        public int CreditCount { get; set; }
    }
}
