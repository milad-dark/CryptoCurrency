using Cryptocurrency.Application.Dtos;

namespace Cryptocurrency.Application.User;

public interface IUserService
{
    Task<UserDto> GetUserByUserNameAndPassword(string username, string password);
    Task SaveNewUser(string username, string password);
}
