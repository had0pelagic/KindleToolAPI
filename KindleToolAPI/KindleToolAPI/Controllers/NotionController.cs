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

        [HttpGet("/test1")]
        public async Task<ActionResult<string>> Test(string secret, string databaseId)
        {
            return Ok(await _notionService.Test(secret, databaseId));
        }
    }
}
