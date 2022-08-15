using Notion.Client;

namespace KindleToolAPI.Services
{
    public interface INotionDatabaseService
    {
        Task<bool> ContainsTitle(NotionClient client, string databaseId, string propertyName, string name);
        Task<Database> GetDatabase(NotionClient client, string databaseId);
        Task QueryDatabase(NotionClient client, string databaseId);
    }
}
