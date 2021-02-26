using LangAppApi.ControllerUtilities;
using LangAppApi.Domain.Auth;
using LangAppApi.Service.Contract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LangAppApi.Controllers.V1
{
    [Route("api/v{version:apiVersion}/accounts")]
    [ApiController]
    [ApiVersion("1.0")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountService"></param>
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("authenticate"), MapToApiVersion("1.0")]
        public async Task<IActionResult> AuthenticateAsync([FromForm] AuthenticationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            return Ok(await _accountService.AuthenticateAsync(request));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("register"), MapToApiVersion("1.0")]
        public async Task<IActionResult> RegisterAsync([FromForm] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            return Ok(await _accountService.RegisterAsync(request));
        }
    }
}