using System.Text.Json.Serialization;

namespace Cryptocurrency.Infrastructure.Models
{
    public record CryptoData([property: JsonPropertyName("quote")] Dictionary<string, Quote> Quote);
}
