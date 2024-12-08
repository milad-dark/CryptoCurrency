using System.Text.Json.Serialization;

namespace Cryptocurrency.Infrastructure.Models
{
    public record ExchangeRatesResponse([property: JsonPropertyName("success")] bool Success, [property: JsonPropertyName("timestamp")] long Timestamp, [property: JsonPropertyName("base")] string Base, [property: JsonPropertyName("date")] DateTimeOffset Date, [property: JsonPropertyName("rates")] Dictionary<string, decimal> Rates);
}
