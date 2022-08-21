using KindleToolAPI.DTOs;
using KindleToolAPI.Models;

namespace KindleToolAPI.Services
{
    public interface IClippingsService
    {
        public Task<List<Clipping>> GetClippings(IClippingsDto dto);
    }
}
