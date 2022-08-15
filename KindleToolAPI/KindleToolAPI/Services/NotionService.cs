using Notion.Client;

namespace KindleToolAPI.Services
{
    public class NotionService : INotionService
    {
        private readonly INotionDatabaseService _databaseService;
        private readonly INotionPageService _pageService;

        public NotionService(INotionDatabaseService databaseService,INotionPageService pageService)
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

            //var pageId = await AddNewPage(client, databaseId, "Page!");
            //await AppendBlockToPage(client, pageId, Blocks.GetHeadingOneBlock("Heading one block!"));
            //await QueryDatabase(client, databaseId);
            var res = await _databaseService.ContainsTitle(client, databaseId, "Books", "Book 1");

            return "test";
        }
    }
}
