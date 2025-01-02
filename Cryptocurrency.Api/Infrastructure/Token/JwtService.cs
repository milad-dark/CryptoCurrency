using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Cryptocurrency.Api.Infrastructure.Helper;
using Microsoft.IdentityModel.Tokens;

namespace Cryptocurrency.Api.Infrastructure.Token;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(string username, string userId)
    {
        int.TryParse(_configuration["JwtSettings:ExpirationMinutes"], out var expirationMinutes);
        var secret = _configuration["JwtSettings:Secret"];

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, username),
            new(ClaimTypes.NameIdentifier, userId)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var encryptionService = new JwtEncryptionService();
        var jwtToken = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: creds
        );

        return encryptionService.GenerateEncryptedJwt(jwtToken.Payload, secret);
    }
}
