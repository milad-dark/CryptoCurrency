using Cryptocurrency.Api.Interfaces;
using System.Security.Claims;

namespace Cryptocurrency.Api.Application
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUsername()
        {
            return _httpContextAccessor.HttpContext?.User.Identity?.Name;
        }

        public string GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User
                .Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
