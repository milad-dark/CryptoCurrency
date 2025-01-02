namespace Cryptocurrency.Application.Dtos;

public class SearchHistoryDto
{
    public int Id
    {
        get; set;
    }
    public int UserId
    {
        get; set;
    }
    public DateTime SearchedAt
    {
        get; set;
    }
    public CryptoSymbolDto CryptoSymbol
    {
        get; set;
    }
}
