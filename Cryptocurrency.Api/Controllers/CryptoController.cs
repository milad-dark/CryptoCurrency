using Cryptocurrency.Api.Application;
using Cryptocurrency.Api.Interfaces;
using Cryptocurrency.Application.Crypto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Cryptocurrency.Api.Controllers
{
    //[Authorize]
    [Route("api/crypto")]
    public class CryptoController : ControllerBase
    {
        private readonly ICryptoApplication _cryptoService;

        public CryptoController(ICryptoApplication cryptoService)
        {
            _cryptoService = cryptoService;
        }

        [HttpGet("GetCrypto/{code}")]
        public async Task<IActionResult> GetCryptoRates(string code)
        {
            try
            {
                //var userId = _userContextService.GetUserId();
                var result = await _cryptoService.GetCryptoSymbolRatesAsync(code,1);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("getlastsearch")]
        public async Task<IActionResult> GetLastSearch()
        {
            //var userId = _userContextService.GetUserId();
            try
            {
                var data = await _cryptoService.GetSearchHistory(1);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
