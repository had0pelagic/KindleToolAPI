using KindleToolAPI.Util.Notion;
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

            var authorTitle = "Author with his book name";
            var childPageName = "2022-02-02 + author and his book name";

            //check if page exists with name
            var parentPageId = await _pageService.PageExistsByName(client, authorTitle);
            if (parentPageId == null)
            {
                parentPageId = await _pageService.AddPage(client, databaseId, authorTitle);
            }
            //check if child page exists with name
            var childPageId = await _pageService.PageExistsByName(client, childPageName);
            if (childPageId == null)
            {
                childPageId = await _pageService.AddChildPage(client, parentPageId, childPageName);
            }

            var texts = new List<string>()
            {
                "“Remember, no man is a failure who has friends.”",
                "If you don't have health, you lack wealth.",
                "If you don't have health, you lack wealth."
            };

            foreach (var text in texts)
            {
                if (await _pageService.ContainsDuplicateParagraph(client, childPageId, text))
                {
                    continue;
                }
                await _pageService.AppendBlockToPage(client, childPageId, Blocks.GetParagraphBlock(text));
            }

            return "test";
        }
    }
}
