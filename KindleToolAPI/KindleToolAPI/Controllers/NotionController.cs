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

        [HttpPost("/clippings-notion")]
        public async Task ClippingsToJson([FromForm] ClippingsFileDto dto, string secret, string databaseId)
        {
            await _notionService.AddClippingsToNotion(dto, secret, databaseId);
        }
    }
}
