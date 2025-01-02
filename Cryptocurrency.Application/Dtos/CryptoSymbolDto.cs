namespace Cryptocurrency.Application.Dtos;

public class CryptoSymbolDto
{
    public string Symbol
    {
        get; set;
    }
    public List<ExchangeRatesDto> ExchangeRates
    {
        get; set;
    }
}
