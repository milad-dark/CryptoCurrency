using Cryptocurrency.Application.Dtos;
using Cryptocurrency.Application.Helper;
using Cryptocurrency.Application.Logger;
using Cryptocurrency.Domain.Interfaces;

namespace Cryptocurrency.Application.User;

public class UserService : IUserService
{
    private readonly ILoggerService<UserService> _logger;
    private readonly IUserRepository _userRepository;
    private readonly PasswordHasher _passwordHasher;

    public UserService(IUserRepository userRepository, ILoggerService<UserService> logger)
    {
        _passwordHasher = new PasswordHasher();
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<UserDto> GetUserByUserNameAndPassword(string username, string password)
    {
        var hashedPassword = _passwordHasher.HashPassword(password);
        var user = await _userRepository.GetUserIdAsync(username, hashedPassword);

        if (user is not null)
        {
            _logger.LogInformation($"Get user:{user}");
            var checkPassword = _passwordHasher.VerifyPassword(password, user.Password);
            if (checkPassword)
                return new UserDto
                {
                    UserId = user.UserId
                };
        }

        return new UserDto();
    }

    public async Task SaveNewUser(string username, string password)
    {
        try
        {
            _logger.LogInformation("Start saving user");
            var hashedPassword = _passwordHasher.HashPassword(password);

            await _userRepository.SaveUser(username, hashedPassword);
            _logger.LogInformation("End saving user");
        }
        catch (Exception ex)
        {
            _logger.LogError("Saving user has error", ex);
        }
    }
}
