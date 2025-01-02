using Cryptocurrency.Api.Infrastructure.Helper;

namespace Cryptocurrency.Tests;

public class EncryptionServiceTests
{
    private readonly JwtEncryptionService _jwtEncryptionService;

    [Fact]
    public void Encrypt_ShouldReturnEncryptedText()
    {
        var key = "SecureKey123456789012345678901234567890";
        var plainText = new Dictionary<string, object> {
            {"1","test1" },
            {"2","test2" },
            {"3","test3" },
        };

        var encryptedText = new JwtEncryptionService().GenerateEncryptedJwt(plainText, key);

        Assert.NotNull(encryptedText);
        Assert.NotEqual(plainText.ToString(), encryptedText);
    }
}
