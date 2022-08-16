using KindleToolAPI.DTOs;
using KindleToolAPI.Services;
using KindleToolAPI.Extensions;
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

        /// <summary>
        /// Convert clippings file text to JSON
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("/clippings-json")]
        public async Task<FileContentResult> ClippingsToJson([FromForm] ClippingsFileDto dto)
        {
            var clippings = await _clippingsService.GetClippings(dto);
            return clippings.ToTextFile("ClippingsJson");
        }
    }
}
