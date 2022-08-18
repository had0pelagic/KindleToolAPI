using KindleToolAPI.DTOs;

namespace KindleToolAPI.Services
{
    public interface INotionService
    {
        public Task AddClippingsToNotion(ClippingsDto dto, string databaseId, string secret);
    }
}
