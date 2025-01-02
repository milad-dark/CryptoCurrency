using Cryptocurrency.Domain.Models;

namespace Cryptocurrency.Domain.Interfaces;

public interface IUserRepository
{
    Task<User> GetUserIdAsync(string username, string password);
    Task SaveUser(string username, string password);
}
