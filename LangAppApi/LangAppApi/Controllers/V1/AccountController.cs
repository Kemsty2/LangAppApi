using LangAppApi.ControllerUtilities;
using LangAppApi.Domain.Auth;
using LangAppApi.Domain.Common;
using LangAppApi.Service.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LangAppApi.Controllers.V1
{
    [Route("api/v{version:apiVersion}/accounts")]
    [ApiController]
    [Produces("application/json")]
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
        /// Authenticate an user
        /// </summary>
        /// <param name="request">Payload for authentication</param>
        /// <returns></returns>
        [HttpPost("authenticate"), MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(AuthenticationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AuthenticateAsync([FromForm] AuthenticationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            return Ok(await _accountService.AuthenticateAsync(request));
        }

        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="request">Payload for registration</param>
        /// <returns></returns>
        [HttpPost("register"), MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterAsync([FromForm] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            return Ok(await _accountService.RegisterAsync(request));
        }
    }
}