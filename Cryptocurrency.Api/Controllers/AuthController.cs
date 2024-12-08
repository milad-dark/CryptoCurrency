using Cryptocurrency.Api.Interfaces;
using Cryptocurrency.Application.User;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocurrency.Api.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;
        public AuthController(IJwtService jwtService, IUserService userService)
        {
            _jwtService = jwtService;
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            var userId = await _userService.GetUserByUserNameAndPassword(request.Username, request.Password);
            if (userId == null)
            {
                return Unauthorized(new { error = "Invalid username or password." });
            }

            if (request.Username == "test" && request.Password == "password")
            {
                var token = _jwtService.GenerateToken(request.Username, "123");
                return Ok(new { token });
            }

            return Unauthorized();
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
