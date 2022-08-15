namespace KindleToolAPI.Services
{
    public interface INotionService
    {
        public Task<string> Test(string key, string databaseId);
    }
}
