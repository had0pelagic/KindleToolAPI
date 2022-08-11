using KindleToolAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace KindleToolAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClippingsController : ControllerBase
    {
        private readonly IClippingsService _clippingsService;

        public ClippingsController(IClippingsService clippingsService)
        {
            _clippingsService = clippingsService;
        }

        [HttpGet("/clippings")]
        public async Task<ActionResult<string>> ClippingsToJson()
        {
            return Ok(await _clippingsService.ClippingsToJson());
        }
    }
}
