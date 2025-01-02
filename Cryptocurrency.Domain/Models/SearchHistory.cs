namespace Cryptocurrency.Domain.Models
{
    public class SearchHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CryptoSymbolId { get; set; }
        public DateTime SearchedAt { get; set; }

        public CryptoSymbol CryptoSymbol { get; set; }
    }
}
