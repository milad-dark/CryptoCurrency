namespace Cryptocurrency.Application.Dtos;

public class ExchangeRatesDto
{
    public string Currency
    {
        get; set;
    }
    public decimal Price
    {
        get; set;
    }
    public DateTime Timestamp
    {
        get; set;
    }
}
