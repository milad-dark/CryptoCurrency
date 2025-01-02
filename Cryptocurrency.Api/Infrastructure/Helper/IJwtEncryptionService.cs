namespace Cryptocurrency.Api.Infrastructure.Helper;

public interface IJwtEncryptionService
{
    string GenerateEncryptedJwt(Dictionary<string, object> payload, string encryptionKey);
    Dictionary<string, object> DecryptJwt(string token, string encryptionKey);
}
