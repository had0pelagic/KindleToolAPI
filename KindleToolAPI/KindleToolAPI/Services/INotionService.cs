using KindleToolAPI.DTOs;

namespace KindleToolAPI.Services
{
    public interface INotionService
    {
        public Task AddClippingsToNotion(ClippingsFileDto dto, string secret, string databaseId);
    }
}
