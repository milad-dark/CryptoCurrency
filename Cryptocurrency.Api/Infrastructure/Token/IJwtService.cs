namespace Cryptocurrency.Api.Infrastructure.Token;

public interface IJwtService
{
    string GenerateToken(string username, string userId);
}
