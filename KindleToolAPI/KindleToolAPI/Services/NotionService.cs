using Notion.Client;

namespace KindleToolAPI.Services
{
    public class NotionService : INotionService
    {
        private readonly INotionDatabaseService _databaseService;
        private readonly INotionPageService _pageService;

        public NotionService(INotionDatabaseService databaseService, INotionPageService pageService)
        {
            _databaseService = databaseService;
            _pageService = pageService;
        }

        public async Task<string> Test(string key, string databaseId)
        {
            var client = NotionClientFactory.Create(new ClientOptions
            {
                AuthToken = key
            });

            var pageId = await _pageService.PageExistsByName(client, "12321");

            return "test";
        }
    }
}
