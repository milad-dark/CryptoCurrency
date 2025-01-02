using System.Security.Claims;
using Cryptocurrency.Api.Infrastructure.Authorization;
using Cryptocurrency.Api.Infrastructure.GeneralApiContracts;
using Cryptocurrency.Api.Infrastructure.Helper;
using Cryptocurrency.Application.Crypto;
using Cryptocurrency.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocurrency.Api.Controllers;

[FilterAuthorize]
[Route("api/crypto")]
public class CryptoController : ControllerBase
{
    private readonly ICryptoApplication _cryptoService;
    private readonly IJwtEncryptionService _jwtEncryptionService;

    public CryptoController(ICryptoApplication cryptoService, IJwtEncryptionService jwtEncryptionService)
    {
        _cryptoService = cryptoService;
        _jwtEncryptionService = jwtEncryptionService;
    }

    [HttpGet("GetCrypto/{code}")]
    public async Task<IActionResult> GetCryptoRates(string code)
    {
        try
        {
            var username = User.Identity?.Name;
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var result = await _cryptoService.GetCryptoSymbolRatesAsync(code, int.Parse(userId));
            return Ok(new ApiResponse<Dictionary<string, decimal>>(result));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse.Error(ex.Message));
        }
    }

    [HttpGet("GetLastSearch")]
    public async Task<IActionResult> GetLastSearch()
    {
        try
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var result = await _cryptoService.GetSearchHistory(int.Parse(userId));
            return Ok(new ApiResponse<IEnumerable<SearchHistoryDto>>(result));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse.Error(ex.Message));
        }
    }
}
