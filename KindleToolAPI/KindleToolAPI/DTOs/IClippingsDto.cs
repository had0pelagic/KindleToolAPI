using KindleToolAPI.Util.Enums;

namespace KindleToolAPI.DTOs
{
    public interface IClippingsDto
    {
        public IFormFile File { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public ClippingTypeEnum Type { get; set; }
    }
}
