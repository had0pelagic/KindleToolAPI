using Notion.Client;

namespace KindleToolAPI.Services
{
    public interface INotionPageService
    {
        Task<string> AddNewPage(NotionClient client, string databaseId, string titleName);
        Task AppendBlockToPage(NotionClient client, string pageId, IBlock block);
        Task AddNewPageToAllPages(NotionClient client, string databaseId);
        Task GetPageBlocks(NotionClient client, string databaseId);
        Task AppendToPages(NotionClient client, string databaseId);
    }
}
