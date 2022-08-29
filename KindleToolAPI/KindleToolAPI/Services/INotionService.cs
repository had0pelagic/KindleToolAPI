using KindleToolAPI.DTOs;

namespace KindleToolAPI.Services
{
    public interface INotionService
    {
        public Task<string> AddClippingsToNotion(ClippingsNotionDto dto);
    }
}
