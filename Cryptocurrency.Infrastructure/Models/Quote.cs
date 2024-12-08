using System.Text.Json.Serialization;

namespace Cryptocurrency.Infrastructure.Models
{
    public record Quote([property: JsonPropertyName("price")] decimal Price);
}
