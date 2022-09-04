using KindleToolAPI.DTOs;
using KindleToolAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace KindleToolAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class NotionController : ControllerBase
    {
        private readonly INotionService _notionService;

        public NotionController(INotionService notionService)
        {
            _notionService = notionService;
        }

        /// <summary>
        /// Sends clippings to given notion database
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="databaseId"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        [HttpPost()]
        public async Task<ActionResult<string>> AddClippingsToNotion([FromForm] ClippingsNotionDto dto)
        {
            var result = await _notionService.AddClippingsToNotion(dto);

            return Ok(result);
        }
    }
}
