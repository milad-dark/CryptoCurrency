namespace Cryptocurrency.Domain.Models
{
    public class CryptoSymbol
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public int UserId { get; set; }


        public List<ExchangeRates> ExchangeRates { get; set; }
    }
}
