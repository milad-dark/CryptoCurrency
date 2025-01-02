using Cryptocurrency.Domain.Interfaces;
using Cryptocurrency.Domain.Models;
using Cryptocurrency.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Cryptocurrency.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationContext _context;

    public UserRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<User> GetUserIdAsync(string username, string password)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username && u.Password == password);
    }

    public async Task SaveUser(string username, string password)
    {
        var user = new User
        {
            UserName = username,
            Password = password,
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }
}
