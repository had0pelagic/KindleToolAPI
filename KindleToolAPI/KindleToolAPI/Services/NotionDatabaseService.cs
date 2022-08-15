using Notion.Client;

namespace KindleToolAPI.Services
{
    public class NotionDatabaseService : INotionDatabaseService
    {
        /// <summary>
        /// Retrieves database data
        /// </summary>
        /// <param name="client"></param>
        /// <param name="databaseId"></param>
        /// <returns></returns>
        public async Task<Database> GetDatabase(NotionClient client, string databaseId)
        {
            return await client.Databases.RetrieveAsync(databaseId);
        }

        /// <summary>
        /// Checks if database contains title with given name
        /// </summary>
        /// <param name="client"></param>
        /// <param name="databaseId"></param>
        /// <param name="propertyName"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<bool> ContainsTitle(NotionClient client, string databaseId, string propertyName, string name)
        {
            var filter = new TitleFilter(propertyName, contains: name);
            var parameters = new DatabasesQueryParameters() { Filter = filter };
            var query = await client.Databases.QueryAsync(databaseId, parameters);

            return query.Results.Count == 1;
        }

        /// <summary>
        /// Gets keys and values of database entries
        /// </summary>
        /// <param name="client"></param>
        /// <param name="databaseId"></param>
        /// <returns></returns>
        public async Task QueryDatabase(NotionClient client, string databaseId)
        {
            var dbQueryParameters = new DatabasesQueryParameters();
            var query = await client.Databases.QueryAsync(databaseId, dbQueryParameters);

            foreach (var result in query.Results)
            {
                foreach (var property in result.Properties)
                {
                    var key = property.Key;
                    var value = property.Value;
                    var name = GetValue(value);
                }
            }
        }

        /// <summary>
        /// Returns property value based on type
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private object GetValue(PropertyValue value)
        {
            switch (value)
            {
                case RichTextPropertyValue richTextPropertyValue://text type
                    return richTextPropertyValue.RichText.FirstOrDefault()?.PlainText;
                case TitlePropertyValue titlePropertyValue://title type
                    return titlePropertyValue.Title.FirstOrDefault()?.PlainText;
                default:
                    return null;
            }
        }
    }
}
