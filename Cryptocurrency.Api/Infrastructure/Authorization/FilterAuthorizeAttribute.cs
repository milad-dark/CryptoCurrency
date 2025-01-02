using System.Security.Claims;
using Cryptocurrency.Api.Infrastructure.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cryptocurrency.Api.Infrastructure.Authorization;

public class FilterAuthorizeAttribute : TypeFilterAttribute
{
    public FilterAuthorizeAttribute() : base(typeof(AuthorizeAttributeFilterAttribute))
    {
    }

    public class AuthorizeAttributeFilterAttribute : IAsyncAuthorizationFilter
    {
        private readonly IJwtEncryptionService _jwtEncryptionService;
        private readonly IConfiguration _configuration;

        public AuthorizeAttributeFilterAttribute(IJwtEncryptionService jwtEncryptionService, IConfiguration configuration)
        {
            _jwtEncryptionService = jwtEncryptionService;
            _configuration = configuration;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            try
            {
                var authHeader = context.HttpContext.Request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                {
                    context.Result = new UnauthorizedObjectResult("token invalid");
                    return;
                }

                var encryptedToken = authHeader.Substring("Bearer ".Length).Trim();
                var secret = _configuration["JwtSettings:Secret"];
                var decryptedToken = _jwtEncryptionService.DecryptJwt(encryptedToken, secret);

                if (decryptedToken == null)
                {
                    context.Result = new UnauthorizedObjectResult("token invalid");
                    return;
                }

                var claims = new List<Claim>
                {
                    new(ClaimTypes.Name, decryptedToken[ClaimTypes.Name].ToString()),
                    new(ClaimTypes.NameIdentifier, decryptedToken[ClaimTypes.NameIdentifier].ToString())
                };

                var identity = new ClaimsIdentity(claims, "Bearer");
                context.HttpContext.User = new ClaimsPrincipal(identity);
                return;
            }
            catch (Exception)
            {
                context.Result = new UnauthorizedObjectResult("token invalid");
                return;
            }
        }
    }
}
