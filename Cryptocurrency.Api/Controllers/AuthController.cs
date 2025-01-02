using Cryptocurrency.Api.Dtos;
using Cryptocurrency.Api.Infrastructure.Authorization;
using Cryptocurrency.Api.Infrastructure.GeneralApiContracts;
using Cryptocurrency.Api.Infrastructure.Token;
using Cryptocurrency.Application.User;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocurrency.Api.Controllers;

[Route("api/auth")]
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
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDto request)
    {
        var user = await _userService.GetUserByUserNameAndPassword(request.Username, request.Password);
        if (user == null)
        {
            return Unauthorized(ApiResponse.Error("Invalid User"));
        }

        var token = _jwtService.GenerateToken(request.Username, user.UserId.ToString());
        return Ok(new ApiResponse<string>(token));
    }

    [FilterAuthorize]
    [HttpPost("RegisterUser")]
    public async Task<IActionResult> AddNewUserAsync([FromBody] LoginRequestDto request)
    {
        await _userService.SaveNewUser(request.Username, request.Password);

        return Ok(ApiResponse.Success("Register user successfull"));
    }
}
