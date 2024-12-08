namespace Cryptocurrency.Domain.Models
{
    public class ExchangeRates
    {
        public int Id { get; set; }
        public string Currency { get; set; }
        public decimal Price { get; set; }
        public DateTime Timestamp { get; set; }
        public int CryptoSymbolId { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public CryptoSymbol CryptoSymbol { get; set; }
    }
}
