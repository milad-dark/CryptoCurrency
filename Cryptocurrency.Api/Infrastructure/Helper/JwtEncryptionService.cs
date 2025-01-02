using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace Cryptocurrency.Api.Infrastructure.Helper;

public class JwtEncryptionService : IJwtEncryptionService
{
    public string GenerateEncryptedJwt(Dictionary<string, object> payload, string encryptionKey)
    {
        try
        {
            var _encryptionKey = EnsureValidKeySize(encryptionKey);
            var payloadJson = System.Text.Json.JsonSerializer.Serialize(payload);
            var encryptedPayload = Encrypt(payloadJson, _encryptionKey);

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var token = new JwtSecurityToken(
                claims: null,
                signingCredentials: null
            //expires: DateTime.UtcNow.AddMinutes(30)
            );

            token.Payload["enc_payload"] = encryptedPayload;
            return jwtTokenHandler.WriteToken(token);
        }
        catch (Exception ex)
        {
            throw new Exception("Encrypt token error", ex);
        }
    }

    public Dictionary<string, object> DecryptJwt(string token, string encryptionKey)
    {
        try
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtTokenHandler.ReadJwtToken(token);

            // Extract encrypted payload
            if (!jwtToken.Payload.TryGetValue("enc_payload", out var encPayloadObj)
                || encPayloadObj is not string encPayload)
            {
                throw new InvalidOperationException("Encrypted payload not found in token.");
            }
            var _encryptionKey = EnsureValidKeySize(encryptionKey);
            // Decrypt payload
            var decryptedPayloadJson = Decrypt(encPayload, _encryptionKey);

            // Deserialize JSON back to dictionary
            return System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(decryptedPayloadJson);
        }
        catch (Exception ex)
        {
            throw new Exception("Decrypt token error", ex);
        }
    }

    private string Encrypt(string plainText, byte[] encryptionKey)
    {
        using var aes = Aes.Create();
        aes.Key = encryptionKey;
        aes.GenerateIV();

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();
        ms.Write(aes.IV, 0, aes.IV.Length);

        using (var cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var writer = new StreamWriter(cryptoStream))
        {
            writer.Write(plainText);
        }

        return Convert.ToBase64String(ms.ToArray());
    }

    private string Decrypt(string encryptedText, byte[] encryptionKey)
    {
        var cipherData = Convert.FromBase64String(encryptedText);

        using var aes = Aes.Create();
        aes.Key = encryptionKey;

        // Extract IV from the cipher data
        var iv = new byte[aes.BlockSize / 8];
        Array.Copy(cipherData, iv, iv.Length);

        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream(cipherData, iv.Length, cipherData.Length - iv.Length);
        using var cryptoStream = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var reader = new StreamReader(cryptoStream);

        return reader.ReadToEnd();
    }

    private byte[] EnsureValidKeySize(string encryptionKey)
    {
        using var sha256 = SHA256.Create();
        return sha256.ComputeHash(Encoding.UTF8.GetBytes(encryptionKey));
    }
}
