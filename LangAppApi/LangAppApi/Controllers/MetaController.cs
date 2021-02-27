using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Extensions.Localization;

namespace LangAppApi.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/meta")]
    public class MetaController : ControllerBase
    {
        private readonly IStringLocalizer<MetaController> _localizer;

        public MetaController(IStringLocalizer<MetaController> localizer)
        {
            _localizer = localizer;
        }

        [HttpGet("/info")]
        public ActionResult<string> Info()
        {
            var assembly = typeof(Startup).Assembly;

            var lastUpdate = System.IO.File.GetLastWriteTime(assembly.Location);
            var version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;


            return Ok($"{_localizer["Version"]}: {version}, {_localizer["Last Updated"]}: {lastUpdate}");
        }
    }
}