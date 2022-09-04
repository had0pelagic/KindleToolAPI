using KindleToolAPI.DTOs;
using KindleToolAPI.Extensions;
using KindleToolAPI.Models;
using KindleToolAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace KindleToolAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ClippingsController : ControllerBase
    {
        private readonly IClippingsService _clippingsService;

        public ClippingsController(IClippingsService clippingsService)
        {
            _clippingsService = clippingsService;
        }

        /// <summary>
        /// Converts clippings file text to JSON and gives out a new file
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("file")]
        public async Task<FileContentResult> ClippingsToJsonFile([FromForm] ClippingsDto dto)
        {
            var result = await _clippingsService.GetClippings(dto);

            return result.ToTextFile("ClippingsJson");
        }

        /// <summary>
        /// Returns clippings as a JSON result
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("json")]
        public async Task<List<Clipping>> ClippingsToJson([FromForm] ClippingsDto dto)
        {
            var result = await _clippingsService.GetClippings(dto);

            return result;
        }

        [HttpGet("test")]
        public async Task<ActionResult<string>> Test()
        {
            return Ok("test returned");
        }
    }
}
