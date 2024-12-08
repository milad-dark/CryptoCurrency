using Cryptocurrency.Application.Dtos;
using Cryptocurrency.Domain.Interfaces;

namespace Cryptocurrency.Application.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> GetUserByUserNameAndPassword(string username, string password)
        {
            var user = await _userRepository.GetUserIdAsync(username, password);

            if (user is not null)
            {
                return new UserDto
                {
                    UserId = user.UserId
                };
            }

            return new UserDto();
        }
    }
}
