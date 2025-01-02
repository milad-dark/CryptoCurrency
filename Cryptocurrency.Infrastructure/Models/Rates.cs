using System.Text.Json.Serialization;

namespace Cryptocurrency.Infrastructure.Models
{
    public record Rates([property: JsonPropertyName("AED")] decimal Aed, [property: JsonPropertyName("AFN")] decimal Afn);
}
