using KindleToolAPI.DTOs;

namespace KindleToolAPI.Services
{
    public interface INotionService
    {
        public Task AddClippingsToNotion(ClippingsNotionDto dto);
    }
}
