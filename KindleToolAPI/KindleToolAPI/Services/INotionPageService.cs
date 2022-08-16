using Notion.Client;

namespace KindleToolAPI.Services
{
    public interface INotionPageService
    {
        Task<string> AddPage(NotionClient client, string databaseId, string titleName);
        Task<string> AddChildPage(NotionClient client, string parentPageId, string titleName);
        Task<bool> PageExistsById(NotionClient client, string pageId);
        Task<string?> PageExistsByName(NotionClient client, string name);
        Task<bool> ContainsDuplicateParagraph(NotionClient client, string pageId, string text);
        Task AppendBlockToPage(NotionClient client, string pageId, IBlock block);
        Task AddNewPageToAllPages(NotionClient client, string databaseId);
        Task GetPageBlocks(NotionClient client, string databaseId);
        Task AppendToPages(NotionClient client, string databaseId);
    }
}
