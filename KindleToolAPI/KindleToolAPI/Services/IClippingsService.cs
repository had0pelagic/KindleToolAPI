using KindleToolAPI.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace KindleToolAPI.Services
{
    public interface IClippingsService
    {
        public Task<FileContentResult> ClippingsToJson(ClippingsFileDto dto);
    }
}
