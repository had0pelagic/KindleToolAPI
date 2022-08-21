using KindleToolAPI.DTOs;
using KindleToolAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace KindleToolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        [HttpPost("/clippings-notion")]
        public async Task<ActionResult<string>> AddClippingsToNotion([FromForm] ClippingsNotionDto dto )
        {
            await _notionService.AddClippingsToNotion(dto);

            return Ok("Success");
        }
    }
}
