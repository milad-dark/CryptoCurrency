namespace Cryptocurrency.Api.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(string username, string userId);
    }
}
